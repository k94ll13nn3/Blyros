using System;
using Microsoft.CodeAnalysis;
using NameInProgress.Entities;
using NameInProgress.Visitors;

namespace NameInProgress.Builders
{
    internal class ClassVisitorBuilder : IClassVisitorBuilder
    {
        public Func<string, bool> NameChecker { get; internal set; }
        public Func<Accessibility, bool> AccessibilityChecker { get; internal set; }
        public Func<ITypeParameterSymbol, bool> GenericParameterChecker { get; internal set; }

        public INameConditionBuilder<IClassVisitorBuilder> WithName()
        {
            return new NameConditionBuilder<IClassVisitorBuilder>(this);
        }

        public IAccessibilityConditionBuilder<IClassVisitorBuilder> WithAccessibility()
        {
            return new AccessibilityConditionBuilder<IClassVisitorBuilder>(this);
        }

        public IGenericParameterConditionBuilder<IClassVisitorBuilder> WithGenericParameter()
        {
            return new GenericParameterConditionBuilder<IClassVisitorBuilder>(this);
        }

        public IVisitor<ClassEntity> Build() => new ClassVisitor(NameChecker, AccessibilityChecker, GenericParameterChecker);
    }
}