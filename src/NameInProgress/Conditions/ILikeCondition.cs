namespace NameInProgress.Conditions
{
    public interface ILikeCondition<T>
    {
        T Like(string value);

        T Like(string value, bool ignoreCase);
    }
}