namespace NameInProgress.Conditions
{
    public interface IAllOfCondition<T, U>
    {
        T AllOf(params U[] values);
    }
}