using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NameInProgress.Interfaces;

namespace NameInProgress
{
    internal class ClassVisitor : SymbolVisitor, IClassVisitorBuilder, IVisitor
    {
        private bool onlyGenerics;
        private bool onlyPublics;
        private string name;
        private ICollection<string> classes;

        public ClassVisitor()
        {
            classes = new List<string>();
        }

        public override void VisitAssembly(IAssemblySymbol symbol)
        {
            symbol.GlobalNamespace.Accept(this);
        }

        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            foreach (INamespaceOrTypeSymbol childSymbol in symbol.GetMembers())
            {
                childSymbol.Accept(this);
            }
        }

        public IClassVisitorBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public IClassVisitorBuilder OnlyGenerics()
        {
            onlyGenerics = true;
            return this;
        }

        public IClassVisitorBuilder OnlyPublics()
        {
            onlyPublics = true;
            return this;
        }

        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            if (onlyPublics && symbol.DeclaredAccessibility != Accessibility.Public)
            {
                return;
            }

            if (onlyGenerics && !symbol.IsGenericType)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(name) && !symbol.Name.Contains(name))
            {
                return;
            }

            classes.Add(symbol.ToString());
        }

        public IVisitor Build() => this;

        public IEnumerable<object> Execute(string location)
        {
            MetadataReference testAssembly = MetadataReference.CreateFromFile(location);
            MetadataReference mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            // For now, private member cannot be seen, but should be solved in 15.7, see https://github.com/dotnet/roslyn/pull/24468.
            Compilation compilation = CSharpCompilation
                .Create(nameof(NameInProgress))
                .WithReferences(mscorlib, testAssembly);

            Visit(compilation.GetAssemblyOrModuleSymbol(testAssembly));

            return classes;
        }

        public IEnumerable<object> GetResult() => classes;
    }
}