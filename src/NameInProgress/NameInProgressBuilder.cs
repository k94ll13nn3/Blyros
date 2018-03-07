using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace NameInProgress
{
    public class NameInProgressBuilder
    {
        private string name;
        private bool onlyPublics;
        private bool onlyGenerics;

        public NameInProgressBuilder GetClasses() => this;

        public NameInProgressBuilder Build() => this;

        public NameInProgressBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public NameInProgressBuilder OnlyGenerics()
        {
            onlyGenerics = true;
            return this;
        }

        public NameInProgressBuilder OnlyPublics()
        {
            onlyPublics = true;
            return this;
        }

        public IEnumerable<object> Execute(string location)
        {
            MetadataReference testAssembly = MetadataReference.CreateFromFile(location);
            MetadataReference mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Compilation compilation = CSharpCompilation
                .Create(nameof(NameInProgress))
                .WithReferences(mscorlib, testAssembly);

            ISymbol assemblySymbol = compilation.GetAssemblyOrModuleSymbol(testAssembly);

            // For now, private member cannot be seen, but should be solved in 15.7, see https://github.com/dotnet/roslyn/pull/24468.
            var visitor = new ClassVisitor(onlyGenerics, onlyPublics, name);
            visitor.Visit(assemblySymbol);

            return visitor.GetResult();
        }
    }
}