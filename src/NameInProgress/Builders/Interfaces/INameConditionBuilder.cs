using NameInProgress.Conditions;

namespace NameInProgress.Builders
{
    public interface INameConditionBuilder<T> :
        IEqualCondition<T, string>,
        ILikeCondition<T, string>
        where T : IBuilder
    {
    }
}