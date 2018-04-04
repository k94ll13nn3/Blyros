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

            var typeSymbol = A.Fake<ITypeSymbol>();
            A.CallTo(() => typeSymbol.ToDisplayString(A<SymbolDisplayFormat>._)).Returns("NameInProgress.Builders.IGenericParameterChecker");
            var typeSymbols = new List<ITypeSymbol> { typeSymbol };

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.ConstraintTypes).Returns(typeSymbols.ToImmutableArray());

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfType_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                new Builder(A.Fake<IGenericParameterChecker>())
                .OfType<IEnumerable<IGenericParameterChecker>>();

            var typeSymbol = A.Fake<ITypeSymbol>();
            A.CallTo(() => typeSymbol.ToDisplayString(A<SymbolDisplayFormat>._)).Returns("NameInProgress.Builders.IGenericParameterChecker");
            var typeSymbols = new List<ITypeSymbol> { typeSymbol };

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.ConstraintTypes).Returns(typeSymbols.ToImmutableArray());

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }
    }
}