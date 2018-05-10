namespace Blyros.Conditions
{
    /// <summary>
    /// Interface for exposing an equal condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    /// <typeparam name="U">The type objects used in the condition.</typeparam>
    public interface IEqualCondition<T, U>
    {
        /// <summary>
        /// Creates a one of condition that match the specified value.
        /// </summary>
        /// <param name="value">The value to match.</param>
        /// <returns>A <typeparamref name="T"/> class to chain.</returns>
        T EqualTo(U value);
    }
}