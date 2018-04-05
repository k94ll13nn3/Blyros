using System.Collections.Generic;
using System.Collections.Immutable;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using NameInProgress.Builders;
using NameInProgress.Conditions;
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
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .AnyType();

            builder.GenericParameterChecker(A.Fake<ITypeParameterSymbol>()).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfType_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<IGenericParameterChecker>();

            var typeParameterSymbol = GetTypeParameterSymbol("NameInProgress.Builders.IGenericParameterChecker");

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData("NameInProgress.IGenericParameterChecker")]
        [InlineData("Microsoft.CodeAnalysis.ITypeSymbol")]
        public void GenericParameterChecker_OfType_ReturnsFalse(string type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<IGenericParameterChecker>();

            var typeParameterSymbol = GetTypeParameterSymbol(type);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeMultipleTypeToFind_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<IGenericParameterChecker>();

            var typeParameterSymbol = GetTypeParameterSymbol("NameInProgress.Builders.IGenericParameterChecker", "NameInProgress.Builders.INamerChecker");

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithGeneric_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<IEnumerable<string>>();

            var typeParameterSymbol = GetTypeParameterSymbol("System.Collections.Generic.IEnumerable<System.String>");

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithGeneric_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<IEnumerable<string>>();

            var typeParameterSymbol = GetTypeParameterSymbol("System.Collections.Generic.IEnumerable<NameInProgress.Builders.IGenericParameterChecker>");

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeOneOfWithoutParameters_GenericParameterCheckerShouldNotBeNull()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .OneOf();

            builder.GenericParameterChecker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(nameof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeOneOfWithoutParameters_ReturnsFalse(string type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .OneOf();

            var typeParameterSymbol = GetTypeParameterSymbol(type);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeOneOfWithNullParameters_GenericParameterCheckerShouldNotBeNull()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .OneOf(null);

            builder.GenericParameterChecker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(nameof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeOneOfWithNullParameters_ReturnsFalse(string type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .OneOf(null);

            var typeParameterSymbol = GetTypeParameterSymbol(type);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Theory]
        [InlineData("System.Collections.Generic.IEnumerable<System.String>")]
        [InlineData("NameInProgress.Builders.IGenericParameterChecker")]
        [InlineData("NameInProgress.Tests.Builders.GenericParameterConditionBuilderTests")]
        public void GenericParameterChecker_OfTypeOneOf_ReturnsTrue(string type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .OneOf(typeof(IGenericParameterChecker), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            var typeParameterSymbol = GetTypeParameterSymbol(type);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData("System.Collections.Generic.IEnumerable<NameInProgress.Builders.IGenericParameterChecker>")]
        [InlineData("Microsoft.CodeAnalysis.ITypeParameterSymbol")]
        public void GenericParameterChecker_OfTypeOneOf_ReturnsFalse(string type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .OneOf(typeof(IGenericParameterChecker), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            var typeParameterSymbol = GetTypeParameterSymbol(type);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeAllOfWithoutParameters_GenericParameterCheckerShouldNotBeNull()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf();

            builder.GenericParameterChecker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(nameof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeAllOfWithoutParameters_ReturnsFalse(string type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf();

            var typeParameterSymbol = GetTypeParameterSymbol(type);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeAllOfWithNullParameters_GenericParameterCheckerShouldNotBeNull()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf(null);

            builder.GenericParameterChecker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(nameof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeAllOfWithNullParameters_ReturnsFalse(string type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf(null);

            var typeParameterSymbol = GetTypeParameterSymbol(type);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeAllOf_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf(typeof(IGenericParameterChecker), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            var typeParameterSymbol = GetTypeParameterSymbol(
                "System.Collections.Generic.IEnumerable<System.String>",
                "NameInProgress.Builders.IGenericParameterChecker",
                "NameInProgress.Tests.Builders.GenericParameterConditionBuilderTests");

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData("System.Collections.Generic.IEnumerable<System.String>")]
        [InlineData("NameInProgress.Builders.IGenericParameterChecker")]
        [InlineData("NameInProgress.Tests.Builders.GenericParameterConditionBuilderTests")]
        public void GenericParameterChecker_OfTypeAllOf_ReturnsFalse(string type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf(typeof(IGenericParameterChecker), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            var typeParameterSymbol = GetTypeParameterSymbol(type);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        private ITypeParameterSymbol GetTypeParameterSymbol(params string[] types)
        {
            var typeSymbols = new List<ITypeSymbol>();
            foreach (var type in types)
            {
                var typeSymbol = A.Fake<ITypeSymbol>();
                A.CallTo(() => typeSymbol.ToDisplayString(A<SymbolDisplayFormat>._)).Returns(type);
                typeSymbols.Add(typeSymbol);
            }

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.ConstraintTypes).Returns(typeSymbols.ToImmutableArray());

            return typeParameterSymbol;
        }
    }
}