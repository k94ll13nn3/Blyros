using NameInProgress.Conditions;
using NameInProgress.Enums;

namespace NameInProgress.Conditions
{
    public interface IAccessibilityCondition<T> :
        IEqualCondition<T, MemberAccessibility>,
        IOneOfCondition<T, MemberAccessibility>
    {
    }
}