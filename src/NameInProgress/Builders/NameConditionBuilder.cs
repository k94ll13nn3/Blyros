using System;
using System.Globalization;

namespace NameInProgress.Builders
{
    internal class NameConditionBuilder<T> : INameConditionBuilder<T> where T : IBuilder
    {
        private T visitor;

        public NameConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        public T Like(string value) => Like(value, false);

        public T Like(string value, bool ignoreCase)
        {
            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.NameChecker = Contains;
                    break;
            }

            return visitor;

            bool Contains(string s)
            {
                return CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                    s,
                    value,
                    ignoreCase ? CompareOptions.OrdinalIgnoreCase : CompareOptions.Ordinal) >= 0;
            }
        }

        public T EqualTo(string value)
        {
            Func<string, bool> nameChecker = s => s == value;
            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.NameChecker = nameChecker;
                    break;
            }

            return visitor;
        }
    }
}