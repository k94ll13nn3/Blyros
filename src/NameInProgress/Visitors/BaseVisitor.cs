using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace NameInProgress.Visitors
{
    internal abstract class BaseVisitor : SymbolVisitor, IVisitor
    {
        public IEnumerable<object> Execute(string location)
        {
            MetadataReference testAssembly = MetadataReference.CreateFromFile(location);

            // not needed if using full framework
            // see if all assemblies are needed
            // https://github.com/dotnet/roslyn/wiki/Runtime-code-generation-using-Roslyn-compilations-in-.NET-Core-App
            IList<MetadataReference> platformAssemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")
                .ToString()
                .Split(';')
                .Select(x => (MetadataReference)MetadataReference.CreateFromFile(x))
                .ToList();
            platformAssemblies.Add(testAssembly);

            // For now, private member cannot be seen, but should be solved in 15.7, see https://github.com/dotnet/roslyn/pull/24468.
            Compilation compilation = CSharpCompilation
                .Create(nameof(NameInProgress))
                .WithReferences(platformAssemblies);

            Visit(compilation.GetAssemblyOrModuleSymbol(testAssembly));

            return GetResults();
        }

        public abstract IEnumerable<object> GetResults();
    }
}