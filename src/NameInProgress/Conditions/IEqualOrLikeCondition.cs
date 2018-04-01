namespace NameInProgress.Conditions
{
    public interface IEqualOrLikeCondition<T> :
        IEqualCondition<T, string>,
        ILikeCondition<T>
    {
    }
}