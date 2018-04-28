using System;
using Microsoft.CodeAnalysis;
using NameInProgress.Conditions;
using NameInProgress.Entities;
using NameInProgress.Visitors;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Builder for a the class visitor.
    /// </summary>
    internal class ClassVisitorBuilder :
        IClassVisitorBuilder,
        IAccessibilityChecker,
        IGenericParameterChecker,
        INameChecker,
        INamespaceChecker
    {
        /// <inheritdoc/>
        public Func<Accessibility, bool> AccessibilityChecker { get; set; }

        /// <inheritdoc/>
        public Func<ITypeParameterSymbol, bool> GenericParameterChecker { get; set; }

        /// <inheritdoc/>
        public Func<string, bool> NameChecker { get; set; }

        /// <inheritdoc/>
        public Func<string, bool> NamespaceChecker { get; set; }

        /// <inheritdoc/>
        public INameCondition<IClassVisitorBuilder> WithName()
        {
            return new NameConditionBuilder<ClassVisitorBuilder, IClassVisitorBuilder>(this);
        }

        /// <inheritdoc/>
        public IAccessibilityCondition<IClassVisitorBuilder> WithAccessibility()
        {
            return new AccessibilityConditionBuilder<ClassVisitorBuilder, IClassVisitorBuilder>(this);
        }

        /// <inheritdoc/>
        public IGenericParameterCondition<IClassVisitorBuilder> WithGenericParameter()
        {
            return new GenericParameterConditionBuilder<ClassVisitorBuilder, IClassVisitorBuilder>(this);
        }

        /// <inheritdoc/>
        public INamespaceCondition<IClassVisitorBuilder> WithNamespace()
        {
            return new NamespaceConditionBuilder<ClassVisitorBuilder, IClassVisitorBuilder>(this);
        }

        /// <inheritdoc/>
        public IVisitor<ClassEntity> Build() => new ClassVisitor(NameChecker, AccessibilityChecker, GenericParameterChecker, NamespaceChecker);
    }
}