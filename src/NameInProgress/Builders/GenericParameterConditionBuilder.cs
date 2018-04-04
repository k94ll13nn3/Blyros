using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using NameInProgress.Conditions;
using NameInProgress.Enums;

namespace NameInProgress.Builders
{
    internal class GenericParameterConditionBuilder<T, TBuilder> :
        IGenericParameterCondition<TBuilder>,
        IAllOfOrOneOfCondition<TBuilder, Type>,
        IEqualOrAllOfOrOneOfCondition<ITypeCondition<TBuilder>, GenericConstraint>
        where T : TBuilder, IGenericParameterChecker
    {
        private T visitor;

        private SymbolDisplayFormat symbolDisplayFormat = new SymbolDisplayFormat(
              typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
              genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);

        private Func<ITypeParameterSymbol, bool> constraintChecker;

        public GenericParameterConditionBuilder(T visitor)
        {
            this.visitor = visitor;
        }

        public TBuilder AnyType()
        {
            visitor.GenericParameterChecker = t => (constraintChecker?.Invoke(t) ?? true);
            return visitor;
        }

        public TBuilder OfType<V>()
        {
            var stringFromType = GetStringFromType(typeof(V));
            visitor.GenericParameterChecker = t =>
                (constraintChecker?.Invoke(t) ?? true) && t.ConstraintTypes.Any(x => x.ToDisplayString(symbolDisplayFormat) == stringFromType);

            return visitor;
        }

        public IAllOfOrOneOfCondition<TBuilder, Type> OfType() => this;

        public TBuilder AllOf(params Type[] values)
        {
            IEnumerable<string> stringsFromTypes = values.Select(type => GetStringFromType(type));
            visitor.GenericParameterChecker = t =>
            {
                IEnumerable<string> constraintTypes = t.ConstraintTypes.Select(y => y.ToDisplayString(symbolDisplayFormat));
                return (constraintChecker?.Invoke(t) ?? true) && stringsFromTypes.All(x => constraintTypes.Contains(x));
            };

            return visitor;
        }

        public TBuilder OneOf(params Type[] values)
        {
            IEnumerable<string> stringsFromTypes = values.Select(type => GetStringFromType(type));
            visitor.GenericParameterChecker = t =>
            {
                IEnumerable<string> constraintTypes = t.ConstraintTypes.Select(y => y.ToDisplayString(symbolDisplayFormat));
                return (constraintChecker?.Invoke(t) ?? true) && stringsFromTypes.Any(x => constraintTypes.Contains(x));
            };

            return visitor;
        }

        public IEqualOrAllOfOrOneOfCondition<ITypeCondition<TBuilder>, GenericConstraint> WithConstraint() => this;

        public ITypeCondition<TBuilder> EqualTo(GenericConstraint value)
        {
            constraintChecker = t => CheckConstraint(t, value);
            return this;
        }

        public ITypeCondition<TBuilder> AllOf(params GenericConstraint[] values)
        {
            constraintChecker = t => values.All(c => CheckConstraint(t, c));
            return this;
        }

        public ITypeCondition<TBuilder> OneOf(params GenericConstraint[] values)
        {
            constraintChecker = t => values.Any(c => CheckConstraint(t, c));
            return this;
        }

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