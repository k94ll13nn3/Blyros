using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface IAccessibilityCondition<T> where T : IBuilder
    {
        IAccessibilityConditionBuilder<T> WithAccessibility();
    }
}