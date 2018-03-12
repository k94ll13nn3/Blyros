using System;
using NameInProgress.Builders;

namespace NameInProgress.Conditions
{
    public interface ITypeCondition<T> : IBuilder where T : IBuilder
    {
        T OfType<U>();

        IAllOrOneOfCondition<T, Type> OfType();

        T AnyType();
    }
}