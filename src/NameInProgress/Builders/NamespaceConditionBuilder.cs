using System;
using NameInProgress.Conditions;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Builder for creating a condition on namespaces.
    /// </summary>
    /// <typeparam name="T">The type of the visitor that will use the condition.</typeparam>
    /// <typeparam name="TBuilder">The type of the object that will be returned at the end of the chain.</typeparam>
    internal class NamespaceConditionBuilder<T, TBuilder> :
        StringConditionBuilder<T, TBuilder>,
        INamespaceCondition<TBuilder>
        where T : TBuilder, INamespaceChecker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceConditionBuilder{T1, T2}"/> class.
        /// </summary>
        /// <param name="visitor">The visitor that will use the condition.</param>
        public NamespaceConditionBuilder(T visitor)
            : base(visitor)
        {
        }

        /// <inheritdoc/>
        public override void SetChecker(Func<string, bool> checker)
        {
            visitor.NamespaceChecker = checker;
        }
    }
}