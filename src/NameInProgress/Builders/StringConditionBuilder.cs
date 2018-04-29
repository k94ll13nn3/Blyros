using System;
using System.Globalization;
using System.Linq;
using NameInProgress.Conditions;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Builder for creating a condition on strings.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the object that will be returned at the end of the chain.</typeparam>
    internal class StringConditionBuilder<TBuilder> :
        IStringCondition<TBuilder>
    {
        /// <summary>
        /// The visitor that will use the condition.
        /// </summary>
        private TBuilder visitor;

        /// <summary>
        /// The visitor that will use the condition.
        /// </summary>
        private Action<Func<string, bool>> setChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringConditionBuilder{T1}"/> class.
        /// </summary>
        /// <param name="visitor">The visitor that will use the condition.</param>
        /// <param name="setChecker">The action to call to set the checher.</param>
        public StringConditionBuilder(TBuilder visitor, Action<Func<string, bool>> setChecker)
        {
            this.visitor = visitor;
            this.setChecker = setChecker;

            setChecker(_ => false);
        }

        /// <inheritdoc/>
        public TBuilder Like(string value) => Like(value, false);

        /// <inheritdoc/>
        public TBuilder Like(string value, bool ignoreCase)
        {
            if (value != null)
            {
                setChecker(Contains);
            }

            return visitor;

            bool Contains(string s)
            {
                return s != null && CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                    s,
                    value,
                    ignoreCase ? CompareOptions.OrdinalIgnoreCase : CompareOptions.Ordinal) >= 0;
            }
        }

        /// <inheritdoc/>
        public TBuilder EqualTo(string value)
        {
            if (value != null)
            {
                setChecker(s => s == value);
            }

            return visitor;
        }

        /// <inheritdoc/>
        public TBuilder OneOf(params string[] values)
        {
            if (values?.Length > 0)
            {
                setChecker(n => values.Contains(n));
            }

            return visitor;
        }
    }
}