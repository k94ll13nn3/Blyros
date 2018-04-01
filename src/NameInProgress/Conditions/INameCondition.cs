using NameInProgress.Conditions;

namespace NameInProgress.Conditions
{
    public interface INameCondition<T> :
        IEqualOrLikeCondition<T>,
        IOneOfCondition<T, string>
    {
    }
}