using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface IEqualCondition<T, U> where T : IBuilder
    {
        T EqualTo(U value);
    }
}