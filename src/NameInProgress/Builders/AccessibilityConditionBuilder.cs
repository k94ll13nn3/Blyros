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
    /// <typeparam name="TBuilder">The type of the object that will be returned at the end of the chain.</typeparam>
    internal class AccessibilityConditionBuilder<TBuilder> :
        IAccessibilityCondition<TBuilder>
    {
        /// <summary>
        /// The visitor that will use the condition.
        /// </summary>
        private TBuilder visitor;

        /// <summary>
        /// The visitor that will use the condition.
        /// </summary>
        private Action<Func<Accessibility, bool>> setChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessibilityConditionBuilder{T1}"/> class.
        /// </summary>
        /// <param name="visitor">The visitor that will use the condition.</param>
        /// <param name="setChecker">The action to call to set the checher.</param>
        public AccessibilityConditionBuilder(TBuilder visitor, Action<Func<Accessibility, bool>> setChecker)
        {
            this.visitor = visitor;
            this.setChecker = setChecker;

            setChecker(_ => false);
        }

        /// <inheritdoc/>
        public TBuilder EqualTo(MemberAccessibility value)
        {
            Accessibility mappedAccessibility = MapAccessibility(value);
            setChecker(a => a == mappedAccessibility);

            return visitor;
        }

        /// <inheritdoc/>
        public TBuilder OneOf(params MemberAccessibility[] values)
        {
            var mappedAccessibilities = new List<Accessibility?>();
            foreach (MemberAccessibility value in values ?? Enumerable.Empty<MemberAccessibility>())
            {
                mappedAccessibilities.Add(MapAccessibility(value));
            }

            if (mappedAccessibilities.Count != 0)
            {
                setChecker(a => mappedAccessibilities.Contains(a));
            }

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