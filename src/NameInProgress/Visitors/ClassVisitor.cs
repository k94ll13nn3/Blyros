using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using NameInProgress.Entities;

namespace NameInProgress.Visitors
{
    internal class ClassVisitor : BaseVisitor<ClassEntity>
    {
        private ICollection<ClassEntity> classes;
        private Func<string, bool> nameChecker;
        private Func<Accessibility, bool> accessibilityChecker;
        private Func<ITypeParameterSymbol, bool> genericParameterChecker;

        public ClassVisitor(Func<string, bool> nameChecker, Func<Accessibility, bool> accessibilityChecker, Func<ITypeParameterSymbol, bool> genericParameterChecker)
        {
            classes = new List<ClassEntity>();
            this.nameChecker = nameChecker;
            this.accessibilityChecker = accessibilityChecker;
            this.genericParameterChecker = genericParameterChecker;
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

            if (genericParameterChecker != null && (symbol.TypeParameters.Length == 0 || symbol.TypeParameters.Any(type => !genericParameterChecker(type))))
            {
                return;
            }

            classes.Add(new ClassEntity { Name = symbol.ToString() });
        }

        public override IEnumerable<ClassEntity> GetResults() => classes;
    }
}