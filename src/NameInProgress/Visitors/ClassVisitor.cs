using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace NameInProgress.Visitors
{
    internal class ClassVisitor : BaseVisitor
    {
        private ICollection<string> classes;
        private Func<string, bool> nameChecker;
        private Func<Accessibility, bool> accessibilityChecker;

        public ClassVisitor(Func<string, bool> nameChecker, Func<Accessibility, bool> accessibilityChecker)
        {
            classes = new List<string>();
            this.nameChecker = nameChecker;
            this.accessibilityChecker = accessibilityChecker;
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
            // Didn't find any information on this...
            if (symbol.Name == "<Module>")
            {
                return;
            }

            // It seems that enums and structs alsor have TypeKind.Class as TypeKind.
            if (symbol.TypeKind != TypeKind.Class)
            {
                return;
            }

            if (nameChecker?.Invoke(symbol.Name) == false)
            {
                return;
            }

            if (accessibilityChecker?.Invoke(symbol.DeclaredAccessibility) == false)
            {
                return;
            }

            classes.Add(symbol.ToString());
        }

        public override IEnumerable<object> GetResults() => classes;
    }
}