using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using NameInProgress.Conditions;
using NameInProgress.Enums;

namespace NameInProgress.Builders
{
    internal class GenericParameterConditionBuilder<T> :
        IGenericParameterConditionBuilder<T>,
        IAllOfOrOneOfCondition<T, Type>,
        IEqualOrAllOfOrOneOfCondition<ITypeCondition<T>, GenericConstraint>
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

        public T AnyType()
        {
            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.GenericParameterChecker = t => (constraintChecker?.Invoke(t) ?? true);
                    break;
            }

            return visitor;
        }

        public T OfType<U>()
        {
            var stringFromType = GetStringFromType(typeof(U));
            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.GenericParameterChecker = t =>
                        (constraintChecker?.Invoke(t) ?? true)
                        && t.ConstraintTypes.Any(x => x.ToDisplayString(symbolDisplayFormat) == stringFromType);
                    break;
            }

            return visitor;
        }

        public IAllOfOrOneOfCondition<T, Type> OfType() => this;

        public T AllOf(params Type[] values)
        {
            IEnumerable<string> stringsFromTypes = values.Select(type => GetStringFromType(type));
            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.GenericParameterChecker = t =>
                    {
                        IEnumerable<string> constraintTypes = t.ConstraintTypes.Select(y => y.ToDisplayString(symbolDisplayFormat));
                        return (constraintChecker?.Invoke(t) ?? true) && stringsFromTypes.All(x => constraintTypes.Contains(x));
                    };

                    break;
            }

            return visitor;
        }

        public T OneOf(params Type[] values)
        {
            IEnumerable<string> stringsFromTypes = values.Select(type => GetStringFromType(type));
            switch (visitor)
            {
                case ClassVisitorBuilder c:
                    c.GenericParameterChecker = t =>
                    {
                        IEnumerable<string> constraintTypes = t.ConstraintTypes.Select(y => y.ToDisplayString(symbolDisplayFormat));
                        return (constraintChecker?.Invoke(t) ?? true) && stringsFromTypes.Any(x => constraintTypes.Contains(x));
                    };

                    break;
            }

            return visitor;
        }

        public IEqualOrAllOfOrOneOfCondition<ITypeCondition<T>, GenericConstraint> WithConstraint() => this;

        public ITypeCondition<T> EqualTo(GenericConstraint value)
        {
            constraintChecker = t => CheckConstraint(t, value);
            return this;
        }

        public ITypeCondition<T> AllOf(params GenericConstraint[] values)
        {
            constraintChecker = t => values.All(c => CheckConstraint(t, c));
            return this;
        }

        public ITypeCondition<T> OneOf(params GenericConstraint[] values)
        {
            constraintChecker = t => values.Any(c => CheckConstraint(t, c));
            return this;
        }

        private static string GetStringFromType(Type type)
        {
            var name = type.Name.Split('`')[0];
            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(type.Namespace))
            {
                builder.Append($"{type.Namespace}.");
            }

            builder.Append(name);
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