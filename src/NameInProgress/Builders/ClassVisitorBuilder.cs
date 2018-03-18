using System;
using Microsoft.CodeAnalysis;
using NameInProgress.Entities;
using NameInProgress.Visitors;

namespace NameInProgress.Builders
{
    internal class ClassVisitorBuilder :
        IClassVisitorBuilder,
        IAccessibilityCondition,
        IGenericParameterCondition,
        INameCondition
    {
        public Func<Accessibility, bool> AccessibilityChecker { get; set; }
        public Func<ITypeParameterSymbol, bool> GenericParameterChecker { get; set; }
        public Func<string, bool> NameChecker { get; set; }

        public INameConditionBuilder<IClassVisitorBuilder> WithName()
        {
            return new NameConditionBuilder<ClassVisitorBuilder, IClassVisitorBuilder>(this);
        }

        public IAccessibilityConditionBuilder<IClassVisitorBuilder> WithAccessibility()
        {
            return new AccessibilityConditionBuilder<ClassVisitorBuilder, IClassVisitorBuilder>(this);
        }

        public IGenericParameterConditionBuilder<IClassVisitorBuilder> WithGenericParameter()
        {
            return new GenericParameterConditionBuilder<ClassVisitorBuilder, IClassVisitorBuilder>(this);
        }

        public IVisitor<ClassEntity> Build() => new ClassVisitor(NameChecker, AccessibilityChecker, GenericParameterChecker);
    }
}