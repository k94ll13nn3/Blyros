using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface IGenericParameterCondition<T> where T : IBuilder
    {
        IGenericParameterConditionBuilder<T> WithGenericParameter();
    }
}