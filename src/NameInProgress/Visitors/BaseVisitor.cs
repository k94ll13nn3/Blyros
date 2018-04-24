using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace NameInProgress.Visitors
{
    /// <summary>
    /// Base class for all visitors.
    /// </summary>
    /// <typeparam name="T">The type of entity to visit.</typeparam>
    internal abstract class BaseVisitor<T> : SymbolVisitor, IVisitor<T>
    {
        /// <inheritdoc/>
        public IEnumerable<T> Execute(string location)
        {
            MetadataReference testAssembly = MetadataReference.CreateFromFile(location);

            IList<MetadataReference> platformAssemblies;
            var trustedPlatformAssemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
            if (trustedPlatformAssemblies != null)
            {
                // .NET Core App need this for resolving types.
                // see if all assemblies are needed
                // https://github.com/dotnet/roslyn/wiki/Runtime-code-generation-using-Roslyn-compilations-in-.NET-Core-App
                platformAssemblies = trustedPlatformAssemblies
                    .ToString()
                    .Split(';')
                    .Select(x => (MetadataReference)MetadataReference.CreateFromFile(x))
                    .ToList();
            }
            else
            {
                MetadataReference mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                platformAssemblies = new List<MetadataReference> { mscorlib };
            }

            platformAssemblies.Add(testAssembly);

            Compilation compilation = CSharpCompilation
                .Create(nameof(NameInProgress))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithMetadataImportOptions(MetadataImportOptions.All))
                .WithReferences(platformAssemblies);

            Visit(compilation.GetAssemblyOrModuleSymbol(testAssembly));

            return GetResults();
        }

        /// <inheritdoc/>
        public IEnumerable<T> Execute(Type type) => Execute(type.Assembly.Location);

        /// <summary>
        /// Gets the result of the execution.
        /// </summary>
        /// <returns>A list of matching entities.</returns>
        public abstract IEnumerable<T> GetResults();

        /// <summary>
        /// Implements the default visit for a symbol. The <see cref="SymbolVisitor.DefaultVisit(ISymbol)"/> method is not overriden to use this implementation.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        protected void DefaultVisitInternal(ISymbol symbol)
        {
            if (symbol is INamespaceOrTypeSymbol namespaceOrTypeSymbol)
            {
                foreach (ISymbol child in namespaceOrTypeSymbol.GetMembers())
                {
                    child.Accept(this);
                }
            }
        }
    }
}