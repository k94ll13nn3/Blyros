namespace Blyros.Conditions
{
    /// <summary>
    /// Interface for exposing a string condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    public interface IStringCondition<T> :
        IEqualOrLikeCondition<T>,
        IOneOfCondition<T, string>
    {
    }
}