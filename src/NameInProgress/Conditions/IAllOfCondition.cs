using System.Collections.Generic;
using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface IAllOfCondition<T, U> where T : IBuilder
    {
        T AllOf(IEnumerable<U> values);

        T AllOf(params U[] values);
    }
}