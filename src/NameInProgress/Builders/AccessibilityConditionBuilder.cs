using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using NameInProgress.Conditions;
using NameInProgress.Enums;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Builder for creating a condition on accessibility.
    /// </summary>
    /// <typeparam name="T">The type of the visitor that will use the condition.</typeparam>
    /// <typeparam name="TBuilder">The type of the object that will be returned at the end of the chain.</typeparam>
    internal class AccessibilityConditionBuilder<T, TBuilder> :
        IAccessibilityCondition<TBuilder>
        where T : TBuilder, IAccessibilityChecker
    {
        /// <summary>
        /// The visitor that will use the condition.
        /// </summary>
        private T visitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessibilityConditionBuilder{T1, T2}"/> class.
        /// </summary>
        /// <param name="visitor">The visitor that will use the condition.</param>
        public AccessibilityConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        /// <inheritdoc/>
        public TBuilder EqualTo(MemberAccessibility value)
        {
            Accessibility mappedAccessibility = MapAccessibility(value);
            visitor.AccessibilityChecker = a => a == mappedAccessibility;

            return visitor;
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Maps a <see cref="MemberAccessibility"/> to a <see cref="Accessibility"/>.
        /// </summary>
        /// <param name="value">The <see cref="MemberAccessibility"/> to map.</param>
        /// <returns>The mapped <see cref="Accessibility"/>.</returns>
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