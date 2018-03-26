using System;
using System.Collections.Generic;
using System.Linq;
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
            Accessibility mappedAccessibility = MapAccessibility(value);
            visitor.AccessibilityChecker = a => a == mappedAccessibility;

            return visitor;
        }

        public TBuilder OneOf(params MemberAccessibility[] values)
        {
            var mappedAccessibilities = new List<Accessibility?>();
            Func<Accessibility, bool> accessibilityChecker;
            foreach (MemberAccessibility value in values ?? Enumerable.Empty<MemberAccessibility>())
            {
                mappedAccessibilities.Add(MapAccessibility(value));
            }

            if (mappedAccessibilities.Count == 0)
            {
                accessibilityChecker = _ => false;
            }
            else
            {
                accessibilityChecker = a => mappedAccessibilities.Contains(a);
            }

            visitor.AccessibilityChecker = accessibilityChecker;

            return visitor;
        }

        private static Accessibility MapAccessibility(MemberAccessibility value)
        {
            switch (value)
            {
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

                case MemberAccessibility.Public:
                    return Accessibility.Public;
            }

            throw new ArgumentException($"'{nameof(value)}' has not a valid value.", nameof(value));
        }
    }
}