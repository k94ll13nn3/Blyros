namespace NameInProgress.Conditions
{
    /// <summary>
    /// Interface for exposing an equal or like condition on strings.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    public interface IEqualOrLikeCondition<T> :
        IEqualCondition<T, string>,
        ILikeCondition<T>
    {
    }
}