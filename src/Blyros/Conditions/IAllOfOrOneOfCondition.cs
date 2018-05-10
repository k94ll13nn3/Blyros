namespace Blyros.Conditions
{
    /// <summary>
    /// Interface for exposing an all of or one of condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    /// <typeparam name="U">The type objects used in the condition.</typeparam>
    public interface IAllOfOrOneOfCondition<T, U> :
        IAllOfCondition<T, U>,
        IOneOfCondition<T, U>
    {
    }
}