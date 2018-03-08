namespace NameInProgress.Interfaces
{
    public interface INameCondition<T> where T : IVisitor
    {
        IEqualOrLikeCondition<T> WithName();
    }
}