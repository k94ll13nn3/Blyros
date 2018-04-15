using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using NameInProgress.Conditions;
using NameInProgress.Enums;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Builder for creating a condition on generic parameters.
    /// </summary>
    /// <typeparam name="T">The type of the visitor that will use the condition.</typeparam>
    /// <typeparam name="TBuilder">The type of the object that will be returned at the end of the chain.</typeparam>
    internal class GenericParameterConditionBuilder<T, TBuilder> :
        IGenericParameterCondition<TBuilder>,
        IAllOfOrOneOfCondition<TBuilder, Type>,
        IEqualOrAllOfOrOneOfCondition<ITypeCondition<TBuilder>, GenericConstraint>
        where T : TBuilder, IGenericParameterChecker
    {
        /// <summary>
        /// The visitor that will use the condition.
        /// </summary>
        private T visitor;

        /// <summary>
        /// The display format used to create a string from a <see cref="ITypeSymbol"/>.
        /// </summary>
        private SymbolDisplayFormat symbolDisplayFormat = new SymbolDisplayFormat(
              typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
              genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);

        /// <summary>
        /// The function used to check constraint.
        /// </summary>
        private Func<ITypeParameterSymbol, bool> constraintChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericParameterConditionBuilder{T1, T2}"/> class.
        /// </summary>
        /// <param name="visitor">The visitor that will use the condition.</param>
        public GenericParameterConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        /// <inheritdoc/>
        public TBuilder AnyType()
        {
            visitor.GenericParameterChecker = t => (constraintChecker?.Invoke(t) ?? true);
            return visitor;
        }

        /// <inheritdoc/>
        public TBuilder OfType<V>()
        {
            var stringFromType = GetStringFromType(typeof(V));
            visitor.GenericParameterChecker = t =>
                (constraintChecker?.Invoke(t) ?? true) && t.ConstraintTypes.Any(x => x.ToDisplayString(symbolDisplayFormat) == stringFromType);

            return visitor;
        }

        /// <inheritdoc/>
        public IAllOfOrOneOfCondition<TBuilder, Type> OfType() => this;

        /// <inheritdoc/>
        public TBuilder AllOf(params Type[] values)
        {
            return AllOrOneOf(values, true);
        }

        /// <inheritdoc/>
        public TBuilder OneOf(params Type[] values)
        {
            return AllOrOneOf(values, false);
        }

        /// <inheritdoc/>
        public IEqualOrAllOfOrOneOfCondition<ITypeCondition<TBuilder>, GenericConstraint> WithConstraint() => this;

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
            else
            {
                constraintChecker = _ => false;
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
            else
            {
                constraintChecker = _ => false;
            }

            return this;
        }

        /// <summary>
        /// Retrurns a string from a <see cref="Type"/>.
        /// </summary>
        /// <remarks>Will match the string returned by the use of <see cref="symbolDisplayFormat"/> on a <see cref="ITypeSymbol"/>.</remarks>
        private static string GetStringFromType(Type type)
        {
            var builder = new StringBuilder();
            builder.Append(type.FullName.Split('`')[0]);
            if (type.GenericTypeArguments.Any())
            {
                builder.Append($"<{string.Join(", ", type.GenericTypeArguments.Select(z => GetStringFromType(z)))}>");
            }

            return builder.ToString();
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

        /// <summary>
        /// Creates a condition for both the AllOf and OneOf methods on types.
        /// </summary>
        /// <param name="values">The lists of values to match.</param>
        /// <param name="isAllOf">A value indicating whether the call if by the AllOf method.</param>
        /// <returns>A <typeparamref name="TBuilder"/> class to chain.</returns>
        private TBuilder AllOrOneOf(Type[] values, bool isAllOf)
        {
            if (values?.Length > 0)
            {
                IEnumerable<string> stringsFromTypes = values.Select(type => GetStringFromType(type));
                visitor.GenericParameterChecker = t =>
                {
                    IEnumerable<string> constraintTypes = t.ConstraintTypes.Select(y => y.ToDisplayString(symbolDisplayFormat));
                    Func<IEnumerable<string>, Func<string, bool>, bool> selector;
                    if (isAllOf)
                    {
                        selector = Enumerable.All;
                    }
                    else
                    {
                        selector = Enumerable.Any;
                    }

                    return (constraintChecker?.Invoke(t) ?? true) && selector(stringsFromTypes, x => constraintTypes.Contains(x));
                };
            }
            else
            {
                visitor.GenericParameterChecker = _ => false;
            }

            return visitor;
        }
    }
}