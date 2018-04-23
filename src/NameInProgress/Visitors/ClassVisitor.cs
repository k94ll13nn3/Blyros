using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using NameInProgress.Entities;

namespace NameInProgress.Visitors
{
    /// <summary>
    /// The visitor for classes.
    /// </summary>
    internal class ClassVisitor : BaseVisitor<ClassEntity>
    {
        /// <summary>
        /// The matching classes.
        /// </summary>
        private ICollection<ClassEntity> classes;

        /// <summary>
        /// The name checking function.
        /// </summary>
        private Func<string, bool> nameChecker;

        /// <summary>
        /// The name accessibility function.
        /// </summary>
        private Func<Accessibility, bool> accessibilityChecker;

        /// <summary>
        /// The name generic parameter function.
        /// </summary>
        private Func<ITypeParameterSymbol, bool> genericParameterChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassVisitor"/> class.
        /// </summary>
        /// <param name="nameChecker">The name checking function.</param>
        /// <param name="accessibilityChecker">The name accessibility function.</param>
        /// <param name="genericParameterChecker">The name generic parameter function.</param>
        public ClassVisitor(Func<string, bool> nameChecker, Func<Accessibility, bool> accessibilityChecker, Func<ITypeParameterSymbol, bool> genericParameterChecker)
        {
            classes = new List<ClassEntity>();
            this.nameChecker = nameChecker;
            this.accessibilityChecker = accessibilityChecker;
            this.genericParameterChecker = genericParameterChecker;
        }

        /// <inheritdoc/>
        public override void VisitAssembly(IAssemblySymbol symbol)
        {
            symbol.GlobalNamespace.Accept(this);
        }

        /// <inheritdoc/>
        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            foreach (INamespaceOrTypeSymbol childSymbol in symbol.GetMembers())
            {
                childSymbol.Accept(this);
            }
        }

        /// <inheritdoc/>
        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            // Didn't find any information on this...
            if (symbol.Name == "<Module>")
            {
                return;
            }

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

            classes.Add(new ClassEntity { Name = symbol.Name, FullName = symbol.ToString() });
        }

        /// <inheritdoc/>
        public override IEnumerable<ClassEntity> GetResults() => classes;
    }
}