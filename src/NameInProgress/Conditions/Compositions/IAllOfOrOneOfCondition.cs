namespace NameInProgress.Conditions
{
    public interface IAllOfOrOneOfCondition<T, U> :
        IAllOfCondition<T, U>,
        IOneOfCondition<T, U>
    {
    }
}