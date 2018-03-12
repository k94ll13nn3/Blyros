using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface IEqualOrLikeCondition<T> :
        IEqualCondition<T, string>,
        ILikeCondition<T>
        where T : IBuilder
    {
    }
}