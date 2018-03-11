using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface ILikeCondition<T> where T : IBuilder
    {
        T Like(string value);

        T Like(string value, bool ignoreCase);
    }
}