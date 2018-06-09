using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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
        /// Initializes a new instance of the <see cref="BlyrosSymbolVisitor"/> class.
        /// </summary>
        private BlyrosSymbolVisitor()
        {
        }

        /// <summary>
        /// Creates a new <see cref="BlyrosSymbolVisitor"/>.
        /// </summary>
        /// <returns>A new <see cref="BlyrosSymbolVisitor"/>.</returns>
        public static BlyrosSymbolVisitor Create() => new BlyrosSymbolVisitor();

        /// <inheritdoc/>
        public override IEnumerable<ISymbol> DefaultVisit(ISymbol symbol)
        {
            var symbols = new List<ISymbol> { symbol };
            if (symbol is INamespaceOrTypeSymbol namespaceOrTypeSymbol)
            {
                ParallelQuery<ISymbol> collection = namespaceOrTypeSymbol
                    .GetMembers()
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

        public IEnumerable<ISymbol> Execute(Type type) => Execute(type.Assembly.Location);

        public IEnumerable<ISymbol> Execute(string path)
        {
            MetadataReference testedAssembly = MetadataReference.CreateFromFile(path);

            // TODO: test other platforms (like Mono) to see what is needed.
            IList<MetadataReference> platformAssemblies;
            object trustedPlatformAssemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
            if (trustedPlatformAssemblies != null)
            {
                // .NET Core App need this for resolving types.
                // TODO: see if all assemblies are needed.
                // https://github.com/dotnet/roslyn/wiki/Runtime-code-generation-using-Roslyn-compilations-in-.NET-Core-App
                platformAssemblies = trustedPlatformAssemblies
                    .ToString()
                    .Split(Path.PathSeparator)
                    //.Where(t => t.Contains("System.Runtime.dll") || t.Contains("System.Private.CoreLib.dll") || t.Contains("netstandard.dll"))
                    .Select(x => (MetadataReference)MetadataReference.CreateFromFile(x))
                    .ToList();
            }
            else
            {
                // .NET Framework
                MetadataReference mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                platformAssemblies = new List<MetadataReference> { mscorlib };
            }

            platformAssemblies.Add(testedAssembly);

            Compilation compilation = CSharpCompilation
                .Create(nameof(BlyrosSymbolVisitor))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithMetadataImportOptions(MetadataImportOptions.All))
                .WithReferences(platformAssemblies);

            return Visit(compilation.GetAssemblyOrModuleSymbol(testedAssembly));
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
    }
}
