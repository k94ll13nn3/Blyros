using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface ILikeCondition<T, U> where T : IBuilder
    {
        T Like(U value);
    }
}