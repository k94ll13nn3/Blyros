using System.Collections.Generic;
using System.Collections.Immutable;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using NameInProgress.Builders;
using Xunit;
using Builder = NameInProgress.Builders.GenericParameterConditionBuilder<NameInProgress.Builders.IGenericParameterChecker, NameInProgress.Builders.IGenericParameterChecker>;

namespace NameInProgress.Tests.Builders
{
    public class GenericParameterConditionBuilderTests
    {
        [Fact]
        public void GenericParameterChecker_AnyType_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                new Builder(A.Fake<IGenericParameterChecker>())
                .AnyType();

            builder.GenericParameterChecker(A.Fake<ITypeParameterSymbol>()).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfType_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                new Builder(A.Fake<IGenericParameterChecker>())
                .OfType<IGenericParameterChecker>();

            var typeSymbols = GetTypeSymbols("NameInProgress.Builders.IGenericParameterChecker");
            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.ConstraintTypes).Returns(typeSymbols);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData("NameInProgress.IGenericParameterChecker")]
        [InlineData("Microsoft.CodeAnalysis.ITypeSymbol")]
        public void GenericParameterChecker_OfType_ReturnsFalse(string type)
        {
            IGenericParameterChecker builder =
                new Builder(A.Fake<IGenericParameterChecker>())
                .OfType<IGenericParameterChecker>();

            var typeSymbols = GetTypeSymbols(type);
            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.ConstraintTypes).Returns(typeSymbols);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeMultipleTypeToFind_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                new Builder(A.Fake<IGenericParameterChecker>())
                .OfType<IGenericParameterChecker>();

            var typeSymbols = GetTypeSymbols("NameInProgress.Builders.IGenericParameterChecker", "NameInProgress.Builders.INamerChecker");
            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.ConstraintTypes).Returns(typeSymbols);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithGeneric_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                new Builder(A.Fake<IGenericParameterChecker>())
                .OfType<IEnumerable<string>>();

            var typeSymbols = GetTypeSymbols("System.Collections.Generic.IEnumerable<System.String>");
            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.ConstraintTypes).Returns(typeSymbols);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithGeneric_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                new Builder(A.Fake<IGenericParameterChecker>())
                .OfType<IEnumerable<string>>();

            var typeSymbols = GetTypeSymbols("System.Collections.Generic.IEnumerable<NameInProgress.Builders.IGenericParameterChecker>");
            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.ConstraintTypes).Returns(typeSymbols);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        private ImmutableArray<ITypeSymbol> GetTypeSymbols(params string[] types)
        {
            var typeSymbols = new List<ITypeSymbol>();
            foreach (var type in types)
            {
                var typeSymbol = A.Fake<ITypeSymbol>();
                A.CallTo(() => typeSymbol.ToDisplayString(A<SymbolDisplayFormat>._)).Returns(type);
                typeSymbols.Add(typeSymbol);
            }

            return typeSymbols.ToImmutableArray();
        }
    }
}