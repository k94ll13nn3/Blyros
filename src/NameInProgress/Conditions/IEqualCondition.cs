namespace NameInProgress.Conditions
{
    public interface IEqualCondition<T, U>
    {
        T EqualTo(U value);
    }
}