using System.Collections.Generic;
using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface IOneOfCondition<T, U> where T : IBuilder
    {
        T OneOf(IEnumerable<U> values);

        T OneOf(params U[] values);
    }
}