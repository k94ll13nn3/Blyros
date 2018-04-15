namespace NameInProgress.Conditions
{
    /// <summary>
    /// Interface for exposing a name condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    public interface INameCondition<T> :
        IEqualOrLikeCondition<T>,
        IOneOfCondition<T, string>
    {
    }
}