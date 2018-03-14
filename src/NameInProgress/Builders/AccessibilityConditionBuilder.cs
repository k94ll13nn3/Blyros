using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using NameInProgress.Enums;

namespace NameInProgress.Builders
{
    internal class AccessibilityConditionBuilder<T> : IAccessibilityConditionBuilder<T>
    {
        private T visitor;

        public AccessibilityConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        public T EqualTo(MemberAccessibility value)
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

            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.AccessibilityChecker = accessibilityChecker;
                    break;
            }

            return visitor;
        }

        public T OneOf(params MemberAccessibility[] values)
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

            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.AccessibilityChecker = accessibilityChecker;
                    break;
            }

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

                default:
                    return null;
            }
        }
    }
}