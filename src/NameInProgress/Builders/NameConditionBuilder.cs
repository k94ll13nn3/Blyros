using System;

namespace NameInProgress.Builders
{
    internal class NameConditionBuilder<T> : INameConditionBuilder<T> where T : IBuilder
    {
        private T visitor;

        public NameConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        public T Like(string value)
        {
            Func<string, bool> nameChecker = s => s.Contains(value);
            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.NameChecker = nameChecker;
                    break;
            }

            return visitor;
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