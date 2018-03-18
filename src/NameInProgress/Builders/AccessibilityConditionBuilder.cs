using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using NameInProgress.Enums;

namespace NameInProgress.Builders
{
    internal class AccessibilityConditionBuilder<T, TBuilder> :
        IAccessibilityConditionBuilder<TBuilder>
        where T : TBuilder, IAccessibilityCondition
    {
        private T visitor;

        public AccessibilityConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        public TBuilder EqualTo(MemberAccessibility value)
        {
            Accessibility? mappedAccessibility = MapAccessibility(value);
            Func<Accessibility, bool> accessibilityChecker;
            if (mappedAccessibility == null)
            {
                accessibilityChecker = _ => true;
            }
            else
            {
                accessibilityChecker = a => a == mappedAccessibility;
            }

            visitor.AccessibilityChecker = accessibilityChecker;

            return visitor;
        }

        public TBuilder OneOf(params MemberAccessibility[] values)
        {
            var mappedAccessibilities = new List<Accessibility?>();
            Func<Accessibility, bool> accessibilityChecker;
            foreach (MemberAccessibility value in values)
            {
                mappedAccessibilities.Add(MapAccessibility(value));
            }

            if (mappedAccessibilities.Count == 0)
            {
                accessibilityChecker = _ => true;
            }
            else
            {
                accessibilityChecker = a => mappedAccessibilities.Contains(a);
            }

            visitor.AccessibilityChecker = accessibilityChecker;

            return visitor;
        }

        private static Accessibility? MapAccessibility(MemberAccessibility value)
        {
            switch (value)
            {
                case MemberAccessibility.Public:
                    return Accessibility.Public;

                case MemberAccessibility.Private:
                    return Accessibility.Private;

                case MemberAccessibility.Internal:
                    return Accessibility.Internal;

                case MemberAccessibility.PrivateProtected:
                    return Accessibility.ProtectedAndInternal;

                case MemberAccessibility.Protected:
                    return Accessibility.Protected;

                case MemberAccessibility.ProtectedInternal:
                    return Accessibility.ProtectedOrInternal;

                default:
                    return null;
            }
        }
    }
}