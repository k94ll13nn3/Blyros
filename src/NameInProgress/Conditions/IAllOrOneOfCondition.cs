using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface IAllOrOneOfCondition<T, U> :
        IAllOfCondition<T, U>,
        IOneOfCondition<T, U>
        where T : IBuilder
    {
    }
}