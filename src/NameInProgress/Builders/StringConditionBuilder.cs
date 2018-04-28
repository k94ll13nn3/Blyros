using System;
using System.Globalization;
using System.Linq;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Builder for creating a condition on strings.
    /// </summary>
    /// <typeparam name="T">The type of the visitor that will use the condition.</typeparam>
    /// <typeparam name="TBuilder">The type of the object that will be returned at the end of the chain.</typeparam>
    internal abstract class StringConditionBuilder<T, TBuilder>
        where T : TBuilder
    {
        /// <summary>
        /// The visitor that will use the condition.
        /// </summary>
        protected T visitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringConditionBuilder{T1, T2}"/> class.
        /// </summary>
        /// <param name="visitor">The visitor that will use the condition.</param>
        public StringConditionBuilder(T visitor)
        {
            this.visitor = visitor;

            SetChecker(_ => false);
        }

        /// <inheritdoc/>
        public TBuilder Like(string value) => Like(value, false);

        /// <inheritdoc/>
        public TBuilder Like(string value, bool ignoreCase)
        {
            if (value != null)
            {
                SetChecker(Contains);
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
                SetChecker(s => s == value);
            }

            return visitor;
        }

        /// <inheritdoc/>
        public TBuilder OneOf(params string[] values)
        {
            if (values?.Length > 0)
            {
                SetChecker(n => values.Contains(n));
            }

            return visitor;
        }

        /// <summary>
        /// Sets the checker function.
        /// </summary>
        /// <param name="checker">The function to use.</param>
        public abstract void SetChecker(Func<string, bool> checker);
    }
}