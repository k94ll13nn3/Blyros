namespace NameInProgress.Conditions
{
    /// <summary>
    /// Interface for exposing a namespace condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    public interface INamespaceCondition<T> :
        IEqualOrLikeCondition<T>,
        IOneOfCondition<T, string>
    {
    }
}