using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface INameCondition<T> where T : IBuilder
    {
        INameConditionBuilder<T> WithName();
    }
}