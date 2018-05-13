using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Blyros.Conditions;
using Microsoft.CodeAnalysis;

namespace Blyros.Builders
{
    /// <summary>
    /// Builder for creating a condition on strings.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the object that will be returned at the end of the chain.</typeparam>
    internal class TypeConditionBuilder<TBuilder> :
        ITypeCondition<TBuilder>,
        IAllOfOrOneOfCondition<TBuilder, Type>
    {
        /// <summary>
        /// The action to call to set the checher.
        /// </summary>
        protected Action<Func<IEnumerable<ITypeSymbol>, bool>> setChecker;

        /// <summary>
        /// The visitor that will use the condition.
        /// </summary>
        protected TBuilder visitor;

        /// <summary>
        /// The display format used to create a string from a <see cref="ITypeSymbol"/>.
        /// </summary>
        private SymbolDisplayFormat symbolDisplayFormat = new SymbolDisplayFormat(
              typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
              genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeConditionBuilder{T1}"/> class.
        /// </summary>
        /// <param name="visitor">The visitor that will use the condition.</param>
        /// <param name="setChecker">The action to call to set the checher.</param>
        public TypeConditionBuilder(TBuilder visitor, Action<Func<IEnumerable<ITypeSymbol>, bool>> setChecker)
        {
            this.visitor = visitor;
            this.setChecker = setChecker;

            setChecker(_ => false);
        }

        /// <inheritdoc/>
        public TBuilder AnyType()
        {
            setChecker(t => true);
            return visitor;
        }

        /// <inheritdoc/>
        public TBuilder OfType<V>()
        {
            var stringFromType = GetStringFromType(typeof(V));
            setChecker(t =>
                t.Any(x => x.ToDisplayString(symbolDisplayFormat) == stringFromType));

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

        /// <summary>
        /// Retrurns a string from a <see cref="Type"/>.
        /// </summary>
        /// <remarks>Will match the string returned by the use of <see cref="symbolDisplayFormat"/> on a <see cref="ITypeSymbol"/>.</remarks>
        private static string GetStringFromType(Type type)
        {
            var builder = new StringBuilder();
            builder.Append(type.FullName.Split('`')[0]);
            if (type.GetGenericArguments().Any())
            {
                builder.Append($"<{string.Join(", ", type.GetGenericArguments().Select(z => GetStringFromType(z)))}>");
            }

            return builder.ToString();
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
                setChecker(t =>
                {
                    IEnumerable<string> types = t.Select(y => y.ToDisplayString(symbolDisplayFormat));
                    Func<IEnumerable<string>, Func<string, bool>, bool> selector = Enumerable.Any;
                    if (isAllOf)
                    {
                        selector = Enumerable.All;
                    }

                    return selector(stringsFromTypes, x => types.Contains(x));
                });
            }

            return visitor;
        }
    }
}