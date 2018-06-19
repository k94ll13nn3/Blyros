using System;
using System.Collections.Generic;
using System.Linq;
using Blyros.Extensions;
using Microsoft.CodeAnalysis;

namespace Blyros
{
    /// <summary>
    /// Custom visitor.
    /// </summary>
    public class BlyrosSymbolVisitor : SymbolVisitor<IEnumerable<ISymbol>>
    {
        /// <summary>
        /// The options used by the visitor.
        /// </summary>
        private BlyrosSymbolVisitorOptions options = BlyrosSymbolVisitorOptions.Default;

        /// <summary>
        /// The filter to use on namespaces.
        /// </summary>
        private Func<INamespaceSymbol, bool> namespaceFilter;

        /// <summary>
        /// The filter to use on methods.
        /// </summary>
        private Func<IMethodSymbol, bool> methodFilter;

        /// <summary>
        /// Prevents creation of new instance outside this class.
        /// </summary>
        private BlyrosSymbolVisitor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlyrosSymbolVisitor"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="BlyrosSymbolVisitor"/> class.</returns>
        public static BlyrosSymbolVisitor Create() => new BlyrosSymbolVisitor();

        /// <summary>
        /// Updates the calling instance to use the specified options.
        /// </summary>
        /// <param name="options">The options to use.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithOptions(BlyrosSymbolVisitorOptions options)
        {
            this.options = options;
            return this;
        }

        /// <summary>
        /// Updates the calling instance to use the specified namespace filter.
        /// </summary>
        /// <param name="namespaceFilter">The filter to use on namespaces.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithNamespaceFilter(Func<INamespaceSymbol, bool> namespaceFilter)
        {
            this.namespaceFilter = namespaceFilter;
            return this;
        }

        /// <summary>
        /// Updates the calling instance to use the specified method filter.
        /// </summary>
        /// <param name="methodFilter">The filter to use on methods.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithMethodFilter(Func<IMethodSymbol, bool> methodFilter)
        {
            this.methodFilter = methodFilter;
            return this;
        }

        /// <inheritdoc/>
        public override IEnumerable<ISymbol> DefaultVisit(ISymbol symbol)
        {
            if (!symbol.IsSpecial() && !CanSymbolBeVisited(symbol))
            {
                return Enumerable.Empty<ISymbol>();
            }

            var symbols = new List<ISymbol>();
            if (!symbol.IsSpecial() && IsSymbolWanted(symbol))
            {
                symbols.Add(symbol);
            }

            if (symbol is INamespaceOrTypeSymbol namespaceOrTypeSymbol)
            {
                ParallelQuery<ISymbol> collection = namespaceOrTypeSymbol
                    .GetMembers()
                    .AsParallel()
                    .SelectMany(child => child.Accept(this));
                symbols.AddRange(collection);
            }

            if (symbol is IMethodSymbol methodSymbol)
            {
                ParallelQuery<ISymbol> collection = methodSymbol
                    .Parameters
                    .AsParallel()
                    .SelectMany(child => child.Accept(this));
                symbols.AddRange(collection);
            }

            if (symbol is INamedTypeSymbol namedTypeSymbol)
            {
                ParallelQuery<ISymbol> collection = namedTypeSymbol
                    .TypeParameters
                    .AsParallel()
                    .SelectMany(child => child.Accept(this));
                symbols.AddRange(collection);
            }

            return symbols;
        }

        /// <inheritdoc/>
        public override IEnumerable<ISymbol> VisitAssembly(IAssemblySymbol symbol)
        {
            return symbol.GlobalNamespace.Accept(this);
        }

        /// <summary>
        /// Determines whether an <see cref="ISymbol"/> must be visited.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <returns>A value indicating whether the <see cref="ISymbol"/> must be visited.</returns>
        private bool CanSymbolBeVisited(ISymbol symbol)
        {
            // Do not visit compiler generated symbols.
            if (symbol.IsCompilerGenerated())
            {
                return false;
            }

            bool result = true;

            if (result && methodFilter != null && symbol is IMethodSymbol methodSymbol)
            {
                result = methodFilter(methodSymbol);
            }

            if (result)
            {
                VisitorDepth depth = options.GetVisitorDepth();
                switch (symbol)
                {
                    case INamespaceSymbol _:
                        result = depth >= VisitorDepth.Namespace;
                        break;

                    case INamedTypeSymbol _:
                        result = depth >= VisitorDepth.NamedType;
                        break;

                    case IMethodSymbol _:
                    case IPropertySymbol _:
                    case IFieldSymbol _:
                        result = depth >= VisitorDepth.Members;
                        break;

                    case IParameterSymbol _:
                        result = depth >= VisitorDepth.Parameters;
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether an <see cref="ISymbol"/> will be returned.
        /// </summary>
        /// <param name="symbol">The symbol to return.</param>
        /// <returns>A value indicating whether the <see cref="ISymbol"/> will be returned.</returns>
        private bool IsSymbolWanted(ISymbol symbol)
        {
            bool result = true;
            switch (symbol)
            {
                case INamedTypeSymbol namedTypeSymbol when namedTypeSymbol.TypeKind == TypeKind.Class:
                    result = options.GetClasses;
                    break;

                case INamedTypeSymbol namedTypeSymbol when namedTypeSymbol.TypeKind == TypeKind.Struct:
                    result = options.GetStructs;
                    break;

                case INamedTypeSymbol namedTypeSymbol when namedTypeSymbol.TypeKind == TypeKind.Enum:
                    result = options.GetEnums;
                    break;

                case INamedTypeSymbol namedTypeSymbol when namedTypeSymbol.TypeKind == TypeKind.Interface:
                    result = options.GetInterfaces;
                    break;

                case IMethodSymbol _:
                    result = options.GetMethods;
                    break;

                case IParameterSymbol _:
                    result = options.GetParameters;
                    break;

                case IPropertySymbol _:
                    result = options.GetProperties;
                    break;

                case IFieldSymbol _:
                    result = options.GetFields;
                    break;

                case INamespaceSymbol _:
                    result = options.GetNamespaces;
                    break;

                case ITypeParameterSymbol _:
                    result = options.GetTypeParameters;
                    break;

                default:
                    break;
            }

            // The namespaceFilter cannot be used to prevent the visitor to visit because 
            // some INamespaceSymbol won't match when using the '.' 
            // Ex: if the match is "Blyros.", the fist INamespaceSymbol will be "Blyros" (because how namespaces works),
            // and it won't match.
            if (result && namespaceFilter != null)
            {
                result = symbol is INamespaceSymbol namespaceSymbol
                    ? namespaceFilter(namespaceSymbol)
                    : namespaceFilter(symbol.ContainingNamespace);
            }

            return result;
        }
    }
}
