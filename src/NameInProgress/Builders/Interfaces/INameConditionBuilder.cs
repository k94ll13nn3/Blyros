using NameInProgress.Conditions;

namespace NameInProgress.Builders
{
    public interface INameConditionBuilder<T> :
        IEqualOrLikeCondition<T>,
        IOneOfCondition<T, string>
    {
    }
}