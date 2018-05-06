using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using FakeItEasy;
using Microsoft.CodeAnalysis;

namespace NameInProgress.Tests
{
    public static class Extensions
    {
        public static ITypeSymbol GetFakeTypeSymbol(this Type type)
        {
            ITypeSymbol typeSymbol = A.Fake<ITypeSymbol>();
            A.CallTo(() => typeSymbol.ToDisplayString(A<SymbolDisplayFormat>._)).Returns(GetStringFromType(type));

            return typeSymbol;
        }

        public static ITypeParameterSymbol GetFakeTypeParameterSymbol(this Type type) => GetFakeTypeParameterSymbolInternal(type);

        public static ITypeParameterSymbol GetFakeTypeParameterSymbol(this Type[] types) => GetFakeTypeParameterSymbolInternal(types);

        private static ITypeParameterSymbol GetFakeTypeParameterSymbolInternal(params Type[] types)
        {
            var typeSymbols = new List<ITypeSymbol>();
            foreach (Type type in types)
            {
                ITypeSymbol typeSymbol = A.Fake<ITypeSymbol>();
                A.CallTo(() => typeSymbol.ToDisplayString(A<SymbolDisplayFormat>._)).Returns(GetStringFromType(type));
                typeSymbols.Add(typeSymbol);
            }

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.ConstraintTypes).Returns(typeSymbols.ToImmutableArray());

            return typeParameterSymbol;
        }

        private static string GetStringFromType(Type type)
        {
            if (type == null)
            {
                return null;
            }

            var builder = new StringBuilder();
            builder.Append(type.FullName.Split('`')[0]);
            if (type.GenericTypeArguments.Any())
            {
                builder.Append($"<{string.Join(", ", type.GenericTypeArguments.Select(z => GetStringFromType(z)))}>");
            }

            return builder.ToString();
        }
    }
}