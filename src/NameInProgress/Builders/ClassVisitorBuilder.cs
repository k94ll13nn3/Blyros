using System;
using Microsoft.CodeAnalysis;
using NameInProgress.Visitors;

namespace NameInProgress.Builders
{
    internal class ClassVisitorBuilder : IClassVisitorBuilder
    {
        public Func<string, bool> NameChecker { get; internal set; }
        public Func<Accessibility, bool> AccessibilityChecker { get; internal set; }

        public INameConditionBuilder<IClassVisitorBuilder> WithName()
        {
            return new NameConditionBuilder<IClassVisitorBuilder>(this);
        }

        public IAccessibilityConditionBuilder<IClassVisitorBuilder> WithAccessibility()
        {
            return new AccessibilityConditionBuilder<IClassVisitorBuilder>(this);
        }

        public IVisitor Build() => new ClassVisitor(NameChecker, AccessibilityChecker);
    }
}