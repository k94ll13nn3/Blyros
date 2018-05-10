using Blyros.Enums;

namespace Blyros.Conditions
{
    /// <summary>
    /// Interface for exposing a generic parameter condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    public interface IGenericParameterCondition<T> :
        ITypeCondition<T>
    {
        /// <summary>
        /// Returns an <see cref="IEqualOrAllOfOrOneOfCondition{T1, T2}"/> to create a condition on constraint.
        /// </summary>
        /// <returns>The <see cref="IEqualOrAllOfOrOneOfCondition{T1, T2}"/>.</returns>
        IEqualOrAllOfOrOneOfCondition<ITypeCondition<T>, GenericConstraint> WithConstraint();
    }
}