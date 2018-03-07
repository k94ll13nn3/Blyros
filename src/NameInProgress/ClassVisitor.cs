using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace NameInProgress
{
    public class ClassVisitor : SymbolVisitor
    {
        private bool onlyGenerics;
        private bool onlyPublics;
        private string name;
        private ICollection<string> classes;

        public ClassVisitor(bool onlyGenerics, bool onlyPublics, string name)
        {
            this.onlyGenerics = onlyGenerics;
            this.onlyPublics = onlyPublics;
            this.name = name;
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

        internal IEnumerable<object> GetResult() => classes;
    }
}