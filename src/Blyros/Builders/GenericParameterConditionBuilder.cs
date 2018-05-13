using System;
using System.Linq;
using Blyros.Conditions;
using Blyros.Enums;
using Microsoft.CodeAnalysis;

namespace Blyros.Builders
{
    /// <summary>
    /// Builder for creating a condition on generic parameters.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the object that will be returned at the end of the chain.</typeparam>
    internal class GenericParameterConditionBuilder<TBuilder> :
        TypeConditionBuilder<TBuilder>,
        IGenericParameterCondition<TBuilder>,
        IEqualOrAllOfOrOneOfCondition<ITypeCondition<TBuilder>, GenericConstraint>
    {
        /// <summary>
        /// The function used to check constraint.
        /// </summary>
        private Func<ITypeParameterSymbol, bool> constraintChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericParameterConditionBuilder{T1}"/> class.
        /// </summary>
        /// <param name="visitor">The visitor that will use the condition.</param>
        /// <param name="customSetChecker">The action to call to set the checher.</param>
        public GenericParameterConditionBuilder(TBuilder visitor, Action<Func<ITypeParameterSymbol, bool>> customSetChecker)
            : base(visitor, _ => { })
        {
            this.visitor = visitor;
            setChecker = typeChecker => customSetChecker(t => (constraintChecker?.Invoke(t) ?? true) && typeChecker(t.ConstraintTypes));

            customSetChecker(_ => false);
        }

        /// <inheritdoc/>
        public IEqualOrAllOfOrOneOfCondition<ITypeCondition<TBuilder>, GenericConstraint> WithConstraint()
        {
            constraintChecker = _ => false;
            return this;
        }

        /// <inheritdoc/>
        public ITypeCondition<TBuilder> EqualTo(GenericConstraint value)
        {
            constraintChecker = t => CheckConstraint(t, value);
            return this;
        }

        /// <inheritdoc/>
        public ITypeCondition<TBuilder> AllOf(params GenericConstraint[] values)
        {
            if (values?.Length > 0)
            {
                constraintChecker = t => values.All(c => CheckConstraint(t, c));
            }

            return this;
        }

        /// <inheritdoc/>
        public ITypeCondition<TBuilder> OneOf(params GenericConstraint[] values)
        {
            if (values?.Length > 0)
            {
                constraintChecker = t => values.Any(c => CheckConstraint(t, c));
            }

            return this;
        }

        /// <summary>
        /// Returns a value indicating whether the specified constraint is matched.
        /// </summary>
        /// <param name="typeParameterSymbol">The <see cref="ITypeParameterSymbol"/> on which the constraint will be checked.</param>
        /// <param name="genericConstraint">The constraint to match.</param>
        /// <returns>A value indicating whether the specified constraint is matched.</returns>
        private static bool CheckConstraint(ITypeParameterSymbol typeParameterSymbol, GenericConstraint genericConstraint)
        {
            switch (genericConstraint)
            {
                case GenericConstraint.Class:
                    return typeParameterSymbol.HasReferenceTypeConstraint;

                case GenericConstraint.Struct:
                    return typeParameterSymbol.HasValueTypeConstraint;

                case GenericConstraint.New:
                    return typeParameterSymbol.HasConstructorConstraint;

                default:
                    return false;
            }
        }
    }
}