using System.Globalization;

namespace NameInProgress.Builders
{
    internal class NameConditionBuilder<T, TBuilder> :
        INameConditionBuilder<TBuilder>
        where T : TBuilder, INameCondition
    {
        private T visitor;

        public NameConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        public TBuilder Like(string value) => Like(value, false);

        public TBuilder Like(string value, bool ignoreCase)
        {
            visitor.NameChecker = Contains;
            return visitor;

            bool Contains(string s)
            {
                return CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                    s,
                    value,
                    ignoreCase ? CompareOptions.OrdinalIgnoreCase : CompareOptions.Ordinal) >= 0;
            }
        }

        public TBuilder EqualTo(string value)
        {
            visitor.NameChecker = s => s == value;
            return visitor;
        }
    }
}