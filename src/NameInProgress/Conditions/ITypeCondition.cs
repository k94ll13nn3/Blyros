using System;

namespace NameInProgress.Conditions
{
    public interface ITypeCondition<T>
    {
        T OfType<U>();

        IAllOfOrOneOfCondition<T, Type> OfType();

        T AnyType();
    }
}