namespace NameInProgress.Interfaces
{
    public interface IEqualCondition<T, U> where T : IBuilder
    {
        T EqualTo(U value);
    }
}