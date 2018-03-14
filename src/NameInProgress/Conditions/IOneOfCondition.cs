namespace NameInProgress.Conditions
{
    public interface IOneOfCondition<T, U>
    {
        T OneOf(params U[] values);
    }
}