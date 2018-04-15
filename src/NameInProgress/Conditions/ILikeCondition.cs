namespace NameInProgress.Conditions
{
    /// <summary>
    /// Interface for exposing a like condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    public interface ILikeCondition<T>
    {
        /// <summary>
        /// Creates a one of condition that match when a string is like the specified value.
        /// </summary>
        /// <param name="value">The value to match.</param>
        /// <returns>A <typeparamref name="T"/> class to chain.</returns>
        T Like(string value);

        /// <summary>
        /// Creates a one of condition that match when a string is like the specified value.
        /// </summary>
        /// <param name="value">The value to match.</param>
        /// <param name="ignoreCase">A value indicating whether the case must be ignored.</param>
        /// <returns>A <typeparamref name="T"/> class to chain.</returns>
        T Like(string value, bool ignoreCase);
    }
}