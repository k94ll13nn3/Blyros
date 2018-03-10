using NameInProgress.Enums;

namespace NameInProgress.Interfaces
{
    public interface IAccessibilityCondition<T> where T : IBuilder
    {
        IEqualCondition<T, MemberAccessibility> WithAccessibility();
    }
}