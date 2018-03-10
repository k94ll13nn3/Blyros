using NameInProgress.Conditions;
using NameInProgress.Enums;

namespace NameInProgress.Builders
{
    public interface IAccessibilityConditionBuilder<T> :
        IEqualCondition<T, MemberAccessibility>,
        IOneOfCondition<T, MemberAccessibility>
        where T : IBuilder
    {
    }
}