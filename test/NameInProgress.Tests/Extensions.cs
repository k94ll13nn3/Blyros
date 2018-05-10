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
        public static ImmutableArray<ITypeSymbol> GetFakeTypeSymbols(this Type type) => ImmutableArray.Create(type.GetFakeTypeSymbolInternal());

        public static ImmutableArray<ITypeSymbol> GetFakeTypeSymbols(this Type[] types) => ImmutableArray.CreateRange(types.Select(t => t.GetFakeTypeSymbolInternal()));

        public static ITypeParameterSymbol GetFakeTypeParameterSymbol(this Type type) => GetFakeTypeParameterSymbolInternal(type);

        public static ITypeParameterSymbol GetFakeTypeParameterSymbol(this Type[] types) => GetFakeTypeParameterSymbolInternal(types);

        public static string GetFormattedString(this Type type)
        {
            if (type == null)
            {
                return null;
            }

            var builder = new StringBuilder();
            builder.Append((type.FullName ?? type.Name).Split('`')[0]);
            if (type.GetGenericArguments().Any())
            {
                builder.Append($"<{string.Join(", ", type.GetGenericArguments().Select(z => z.GetFormattedString()))}>");
            }

            return builder.ToString();
        }

        private static ITypeSymbol GetFakeTypeSymbolInternal(this Type type)
        {
            ITypeSymbol typeSymbol = A.Fake<ITypeSymbol>();
            A.CallTo(() => typeSymbol.ToDisplayString(A<SymbolDisplayFormat>._)).Returns(type.GetFormattedString());

            return typeSymbol;
        }

        private static ITypeParameterSymbol GetFakeTypeParameterSymbolInternal(params Type[] types)
        {
            var typeSymbols = new List<ITypeSymbol>();
            foreach (Type type in types)
            {
                ITypeSymbol typeSymbol = A.Fake<ITypeSymbol>();
                A.CallTo(() => typeSymbol.ToDisplayString(A<SymbolDisplayFormat>._)).Returns(type.GetFormattedString());
                typeSymbols.Add(typeSymbol);
            }

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.ConstraintTypes).Returns(typeSymbols.ToImmutableArray());

            return typeParameterSymbol;
        }
    }
}