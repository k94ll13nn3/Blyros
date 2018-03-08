namespace NameInProgress.Interfaces
{
    public interface IEqualOrLikeCondition<T> where T : IVisitor
    {
        T Like(string value);

        T EqualTo(string value);
    }
}