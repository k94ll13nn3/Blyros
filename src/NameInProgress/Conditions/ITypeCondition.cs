using System;

namespace NameInProgress.Conditions
{
    /// <summary>
    /// Interface for exposing a type condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    public interface ITypeCondition<T>
    {
        /// <summary>
        /// Creates a condition that match the specified type.
        /// </summary>
        /// <typeparam name="U">The type to match.</typeparam>
        /// <returns>A <typeparamref name="T"/> class to chain.</returns>
        T OfType<U>();

        /// <summary>
        /// Returns an <see cref="IAllOfOrOneOfCondition{T1, T2}"/> to create a condition on multiples types.
        /// </summary>
        /// <returns>The <see cref="IAllOfOrOneOfCondition{T1, T2}"/>.</returns>
        IAllOfOrOneOfCondition<T, Type> OfType();

        /// <summary>
        /// Creates a condition that match any type.
        /// </summary>
        /// <returns>A <typeparamref name="T"/> class to chain.</returns>
        T AnyType();
    }
}