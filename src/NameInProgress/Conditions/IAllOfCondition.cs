using System.Collections.Generic;

namespace NameInProgress.Conditions
{
    public interface IAllOfCondition<T, U>
    {
        T AllOf(IEnumerable<U> values);

        T AllOf(params U[] values);
    }
}