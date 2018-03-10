namespace NameInProgress.Interfaces
{
    public interface IEqualOrLikeCondition<T, U> : IEqualCondition<T, U> where T : IBuilder
    {
        T Like(U value);
    }
}