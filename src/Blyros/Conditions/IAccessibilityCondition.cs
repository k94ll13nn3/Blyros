using Blyros.Enums;

namespace Blyros.Conditions
{
    /// <summary>
    /// Interface for exposing an accessibility condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    public interface IAccessibilityCondition<T> :
        IEqualCondition<T, MemberAccessibility>,
        IOneOfCondition<T, MemberAccessibility>
    {
    }
}