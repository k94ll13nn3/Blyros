namespace NameInProgress.Conditions
{
    public interface IEqualOrAllOfOrOneOfCondition<T, U> :
        IEqualCondition<T, U>,
        IAllOfOrOneOfCondition<T, U>
    {
    }
}