using System;
using Microsoft.CodeAnalysis;
using NameInProgress.Conditions;
using NameInProgress.Entities;
using NameInProgress.Visitors;

namespace NameInProgress.Builders
{
    internal class ClassVisitorBuilder :
        IClassVisitorBuilder,
        IAccessibilityChecker,
        IGenericParameterChecker,
        INameChecker
    {
        public Func<Accessibility, bool> AccessibilityChecker { get; set; }
        public Func<ITypeParameterSymbol, bool> GenericParameterChecker { get; set; }
        public Func<string, bool> NameChecker { get; set; }

        public INameCondition<IClassVisitorBuilder> WithName()
        {
            return new NameConditionBuilder<ClassVisitorBuilder, IClassVisitorBuilder>(this);
        }

        public IAccessibilityCondition<IClassVisitorBuilder> WithAccessibility()
        {
            return new AccessibilityConditionBuilder<ClassVisitorBuilder, IClassVisitorBuilder>(this);
        }

        public IGenericParameterCondition<IClassVisitorBuilder> WithGenericParameter()
        {
            return new GenericParameterConditionBuilder<ClassVisitorBuilder, IClassVisitorBuilder>(this);
        }

        public IVisitor<ClassEntity> Build() => new ClassVisitor(NameChecker, AccessibilityChecker, GenericParameterChecker);
    }
}