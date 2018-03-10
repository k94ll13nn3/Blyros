using System;
using Microsoft.CodeAnalysis;
using NameInProgress.Enums;
using NameInProgress.Interfaces;

namespace NameInProgress.Builders
{
    internal class AccessibilityConditionBuilder<T> : IEqualCondition<T, MemberAccessibility> where T : IBuilder
    {
        private T visitor;

        public AccessibilityConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        public T EqualTo(MemberAccessibility value)
        {
            // Not really a fan of this...
            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.AccessibilityChecker = GetAccessibilityChecker(value);
                    break;
            }

            return visitor;
        }

        private static Func<Accessibility, bool> GetAccessibilityChecker(MemberAccessibility value)
        {
            switch (value)
            {
                case MemberAccessibility.Public:
                    return a => a == Accessibility.Public;

                case MemberAccessibility.Private:
                    return a => a == Accessibility.Private;

                case MemberAccessibility.Internal:
                    return a => a == Accessibility.Internal;

                default:
                    return _ => true;
            }
        }
    }
}