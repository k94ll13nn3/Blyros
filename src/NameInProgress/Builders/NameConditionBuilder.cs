using System.Globalization;
using System.Linq;
using NameInProgress.Conditions;

namespace NameInProgress.Builders
{
    internal class NameConditionBuilder<T, TBuilder> :
        INameCondition<TBuilder>
        where T : TBuilder, INameChecker
    {
        private T visitor;

        public NameConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        public TBuilder Like(string value) => Like(value, false);

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