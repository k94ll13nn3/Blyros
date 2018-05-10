namespace Blyros.Conditions
{
    /// <summary>
    /// Interface for exposing an equal or all of condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    /// <typeparam name="U">The type objects used in the condition.</typeparam>
    public interface IAllOfCondition<T, U>
    {
        /// <summary>
        /// Creates a one of condition that match all of the object provided. If <paramref name="values"/> is null or empty, the match will fail.
        /// </summary>
        /// <param name="values">The lists of values to match.</param>
        /// <returns>A <typeparamref name="T"/> class to chain.</returns>
        T AllOf(params U[] values);
    }
}