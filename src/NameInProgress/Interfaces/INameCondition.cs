namespace NameInProgress.Interfaces
{
    public interface INameCondition<T> where T : IBuilder
    {
        IEqualOrLikeCondition<T, string> WithName();
    }
}