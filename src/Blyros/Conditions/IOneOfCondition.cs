namespace Blyros.Conditions
{
    /// <summary>
    /// Interface for exposing a one of condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    /// <typeparam name="U">The type objects used in the condition.</typeparam>
    public interface IOneOfCondition<T, U>
    {
        /// <summary>
        /// Creates a one of condition that match one of the object provided. If <paramref name="values"/> is null or empty, the match will fail.
        /// </summary>
        /// <param name="values">The lists of values to match.</param>
        /// <returns>A <typeparamref name="T"/> class to chain.</returns>
        T OneOf(params U[] values);
    }
}