using NameInProgress.Conditions;
using NameInProgress.Enums;

namespace NameInProgress.Conditions
{
    public interface IGenericParameterCondition<T> :
        ITypeCondition<T>
    {
        IEqualOrAllOfOrOneOfCondition<ITypeCondition<T>, GenericConstraint> WithConstraint();
    }
}