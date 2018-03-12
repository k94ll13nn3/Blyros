using NameInProgress.Conditions;

namespace NameInProgress.Builders
{
    public interface IGenericParameterConditionBuilder<T> :
        ITypeCondition<T>
        where T : IBuilder
    {
        IConstraintConditionBuilder<ITypeCondition<T>> WithConstraint();
    }

    public interface IConstraintConditionBuilder<T> :
        IEqualCondition<T, bool>,
        IAllOrOneOfCondition<T, bool>
        where T : IBuilder
    {
    }
}