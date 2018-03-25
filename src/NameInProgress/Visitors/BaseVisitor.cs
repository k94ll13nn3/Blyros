using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace NameInProgress.Visitors
{
    internal abstract class BaseVisitor<T> : SymbolVisitor, IVisitor<T>
    {
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

            // For now, private member cannot be seen, but should be solved in 15.7, see https://github.com/dotnet/roslyn/pull/24468.
            Compilation compilation = CSharpCompilation
                .Create(nameof(NameInProgress))
                .WithReferences(platformAssemblies);

            Visit(compilation.GetAssemblyOrModuleSymbol(testAssembly));

            return GetResults();
        }

        public IEnumerable<T> Execute(Type type) => Execute(type.Assembly.Location);

        public abstract IEnumerable<T> GetResults();
    }
}