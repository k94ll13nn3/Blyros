using System.Globalization;
using System.Linq;
using NameInProgress.Conditions;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Builder for creating a condition on names.
    /// </summary>
    /// <typeparam name="T">The type of the visitor that will use the condition.</typeparam>
    /// <typeparam name="TBuilder">The type of the object that will be returned at the end of the chain.</typeparam>
    internal class NameConditionBuilder<T, TBuilder> :
        INameCondition<TBuilder>
        where T : TBuilder, INameChecker
    {
        /// <summary>
        /// The visitor that will use the condition.
        /// </summary>
        private T visitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NameConditionBuilder{T1, T2}"/> class.
        /// </summary>
        /// <param name="visitor">The visitor that will use the condition.</param>
        public NameConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        /// <inheritdoc/>
        public TBuilder Like(string value) => Like(value, false);

        /// <inheritdoc/>
        public TBuilder Like(string value, bool ignoreCase)
        {
            if (value == null)
            {
                visitor.NameChecker = _ => false;
            }
            else
            {
                visitor.NameChecker = Contains;
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
            if (value == null)
            {
                visitor.NameChecker = _ => false;
            }
            else
            {
                visitor.NameChecker = s => s == value;
            }

            return visitor;
        }

        /// <inheritdoc/>
        public TBuilder OneOf(params string[] values)
        {
            if (values?.Length > 0)
            {
                visitor.NameChecker = n => values.Contains(n);
            }
            else
            {
                visitor.NameChecker = _ => false;
            }

            return visitor;
        }
    }
}