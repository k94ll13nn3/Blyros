using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NameInProgress.Builders;
using NameInProgress.Interfaces;

namespace NameInProgress
{
    internal class ClassVisitorBuilder : SymbolVisitor, IClassVisitorBuilder
    {
        private ICollection<string> classes;

        public ClassVisitorBuilder()
        {
            classes = new List<string>();
        }

        public Func<string, bool> NameChecker { get; internal set; }

        public IEqualOrLikeCondition<IClassVisitorBuilder> WithName()
        {
            return new NameConditionBuilder<IClassVisitorBuilder>(this);
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

        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            if (NameChecker?.Invoke(symbol.Name) == false)
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
    }
}