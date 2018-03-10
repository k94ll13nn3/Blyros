using System;
using Microsoft.CodeAnalysis;
using NameInProgress.Builders;
using NameInProgress.Enums;
using NameInProgress.Interfaces;
using NameInProgress.Visitors;

namespace NameInProgress
{
    internal class ClassVisitorBuilder : IClassVisitorBuilder
    {
        public Func<string, bool> NameChecker { get; internal set; }
        public Func<Accessibility, bool> AccessibilityChecker { get; internal set; }

        public IEqualOrLikeCondition<IClassVisitorBuilder, string> WithName()
        {
            return new NameConditionBuilder<IClassVisitorBuilder>(this);
        }

        public IEqualCondition<IClassVisitorBuilder, MemberAccessibility> WithAccessibility()
        {
            return new AccessibilityConditionBuilder<IClassVisitorBuilder>(this);
        }

        public IVisitor Build() => new ClassVisitor(NameChecker, AccessibilityChecker);
    }
}