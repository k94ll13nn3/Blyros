using NameInProgress.Conditions;
using NameInProgress.Enums;

namespace NameInProgress.Builders
{
    public interface IGenericParameterConditionBuilder<T> :
        ITypeCondition<T>
    {
        IEqualOrAllOfOrOneOfCondition<ITypeCondition<T>, GenericConstraint> WithConstraint();
    }
}