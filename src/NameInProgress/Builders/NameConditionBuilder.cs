using NameInProgress.Interfaces;

namespace NameInProgress.Builders
{
    internal class NameConditionBuilder<T> : IEqualOrLikeCondition<T, string> where T : IBuilder
    {
        private T visitor;

        public NameConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        public T Like(string value)
        {
            // Not really a fan of this...
            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.NameChecker = s => s.Contains(value);
                    break;
            }

            return visitor;
        }

        public T EqualTo(string value)
        {
            // Not really a fan of this...
            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.NameChecker = s => s == value;
                    break;
            }

            return visitor;
        }
    }
}