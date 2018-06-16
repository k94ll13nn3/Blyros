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
            bool result = !symbol.IsCompilerGenerated();

            if (result && symbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.TypeKind == TypeKind.Class)
            {
                result = options.GetClasses;
            }

            if (result && symbol is IMethodSymbol)
            {
                result = options.GetMethods;
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
