using NameInProgress.Conditions;
using NameInProgress.Enums;

namespace NameInProgress.Builders
{
    // TODO : add allof for protected internal/private protected or use the enum ? 
    public interface IAccessibilityConditionBuilder<T> :
        IEqualCondition<T, MemberAccessibility>,
        IOneOfCondition<T, MemberAccessibility>
    {
    }
}