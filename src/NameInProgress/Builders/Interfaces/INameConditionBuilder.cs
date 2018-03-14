using NameInProgress.Conditions;

namespace NameInProgress.Builders
{
    // TODO : add oneof ? 
    public interface INameConditionBuilder<T> :
        IEqualCondition<T, string>,
        ILikeCondition<T>
    {
    }
}