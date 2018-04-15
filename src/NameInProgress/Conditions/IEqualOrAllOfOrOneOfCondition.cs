namespace NameInProgress.Conditions
{
    /// <summary>
    /// Interface for exposing an equal or all of or one of condition.
    /// </summary>
    /// <typeparam name="T">The type of the class to chain.</typeparam>
    /// <typeparam name="U">The type objects used in the condition.</typeparam>
    public interface IEqualOrAllOfOrOneOfCondition<T, U> :
        IEqualCondition<T, U>,
        IAllOfOrOneOfCondition<T, U>
    {
    }
}