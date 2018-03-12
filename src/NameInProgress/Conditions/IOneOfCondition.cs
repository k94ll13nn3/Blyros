using System.Collections.Generic;

namespace NameInProgress.Conditions
{
    public interface IOneOfCondition<T, U>
    {
        T OneOf(IEnumerable<U> values);

        T OneOf(params U[] values);
    }
}