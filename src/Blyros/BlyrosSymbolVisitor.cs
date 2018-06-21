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
        /// The filter to use on classes.
        /// </summary>
        private Func<INamedTypeSymbol, bool> classFilter;

        /// <summary>
        /// The filter to use on parameters.
        /// </summary>
        private Func<IParameterSymbol, bool> parameterFilter;

        /// <summary>
        /// The filter to use on properties.
        /// </summary>
        private Func<IPropertySymbol, bool> propertyFilter;

        /// <summary>
        /// The filter to use on fields.
        /// </summary>
        private Func<IFieldSymbol, bool> fieldFilter;

        /// <summary>
        /// The filter to use on interfaces.
        /// </summary>
        private Func<INamedTypeSymbol, bool> interfaceFilter;

        /// <summary>
        /// The filter to use on structs.
        /// </summary>
        private Func<INamedTypeSymbol, bool> structFilter;

        /// <summary>
        /// The filter to use on enums.
        /// </summary>
        private Func<INamedTypeSymbol, bool> enumFilter;

        /// <summary>
        /// The filter to use on typeParameters.
        /// </summary>
        private Func<ITypeParameterSymbol, bool> typeParameterFilter;

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
        /// Updates the calling instance to use the specified class filter.
        /// </summary>
        /// <param name="classFilter">The filter to use on classes.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithClassFilter(Func<INamedTypeSymbol, bool> classFilter)
        {
            this.classFilter = classFilter;
            return this;
        }

        /// <summary>
        /// Updates the calling instance to use the specified parameter filter.
        /// </summary>
        /// <param name="parameterFilter">The filter to use on parameters.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithParameterFilter(Func<IParameterSymbol, bool> parameterFilter)
        {
            this.parameterFilter = parameterFilter;
            return this;
        }

        /// <summary>
        /// Updates the calling instance to use the specified property filter.
        /// </summary>
        /// <param name="propertyFilter">The filter to use on properties.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithPropertyFilter(Func<IPropertySymbol, bool> propertyFilter)
        {
            this.propertyFilter = propertyFilter;
            return this;
        }

        /// <summary>
        /// Updates the calling instance to use the specified field filter.
        /// </summary>
        /// <param name="fieldFilter">The filter to use on fields.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithFieldFilter(Func<IFieldSymbol, bool> fieldFilter)
        {
            this.fieldFilter = fieldFilter;
            return this;
        }

        /// <summary>
        /// Updates the calling instance to use the specified interface filter.
        /// </summary>
        /// <param name="interfaceFilter">The filter to use on interfaces.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithInterfaceFilter(Func<INamedTypeSymbol, bool> interfaceFilter)
        {
            this.interfaceFilter = interfaceFilter;
            return this;
        }

        /// <summary>
        /// Updates the calling instance to use the specified struct filter.
        /// </summary>
        /// <param name="structFilter">The filter to use on structs.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithStructFilter(Func<INamedTypeSymbol, bool> structFilter)
        {
            this.structFilter = structFilter;
            return this;
        }

        /// <summary>
        /// Updates the calling instance to use the specified enum filter.
        /// </summary>
        /// <param name="enumFilter">The filter to use on enums.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithEnumFilter(Func<INamedTypeSymbol, bool> enumFilter)
        {
            this.enumFilter = enumFilter;
            return this;
        }

        /// <summary>
        /// Updates the calling instance to use the specified typeParameter filter.
        /// </summary>
        /// <param name="typeParameterFilter">The filter to use on typeParameters.</param>
        /// <returns>The calling instance updated.</returns>
        public BlyrosSymbolVisitor WithTypeparameterFilter(Func<ITypeParameterSymbol, bool> typeParameterFilter)
        {
            this.typeParameterFilter = typeParameterFilter;
            return this;
        }

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
            VisitorDepth depth = options.GetVisitorDepth();
            switch (symbol)
            {
                case INamespaceSymbol _:
                    result = depth >= VisitorDepth.Namespace;
                    break;

                case INamedTypeSymbol namedTypeSymbol when namedTypeSymbol.TypeKind == TypeKind.Class:
                    result = depth >= VisitorDepth.NamedType && classFilter?.Invoke(namedTypeSymbol) != false;
                    break;

                case INamedTypeSymbol namedTypeSymbol when namedTypeSymbol.TypeKind == TypeKind.Struct:
                    result = depth >= VisitorDepth.NamedType && structFilter?.Invoke(namedTypeSymbol) != false;
                    break;

                case INamedTypeSymbol namedTypeSymbol when namedTypeSymbol.TypeKind == TypeKind.Enum:
                    result = depth >= VisitorDepth.NamedType && enumFilter?.Invoke(namedTypeSymbol) != false;
                    break;

                case INamedTypeSymbol namedTypeSymbol when namedTypeSymbol.TypeKind == TypeKind.Interface:
                    result = depth >= VisitorDepth.NamedType && interfaceFilter?.Invoke(namedTypeSymbol) != false;
                    break;

                case IMethodSymbol methodSymbol:
                    result = depth >= VisitorDepth.Members && methodFilter?.Invoke(methodSymbol) != false;
                    break;

                case IPropertySymbol propertySymbol:
                    result = depth >= VisitorDepth.Members && propertyFilter?.Invoke(propertySymbol) != false;
                    break;

                case IFieldSymbol fieldSymbol:
                    result = depth >= VisitorDepth.Members && fieldFilter?.Invoke(fieldSymbol) != false;
                    break;

                case IParameterSymbol parameterSymbol:
                    result = depth >= VisitorDepth.Parameters && parameterFilter?.Invoke(parameterSymbol) != false;
                    break;

                case ITypeParameterSymbol typeParameterSymbol:
                    result = typeParameterFilter(typeParameterSymbol);
                    break;
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
