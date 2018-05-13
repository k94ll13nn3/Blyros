using System;
using System.Collections.Generic;
using System.Linq;
using Blyros.Builders;
using Blyros.Conditions;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Blyros.Tests.Builders
{
    public class TypeConditionBuilderTests
    {
        [Fact]
        public void TypeChecker_AnyType_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .AnyType();

            fakeBuilder.Checker(Enumerable.Empty<ITypeSymbol>()).Should().BeTrue();
        }

        [Fact]
        public void TypeChecker_OfType_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<ITypeCondition<object>>();

            IEnumerable<ITypeSymbol> typeSymbols = typeof(ITypeCondition<object>).GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(ITypeCondition<ITypeCondition<object>>))]
        [InlineData(typeof(ITypeSymbol))]
        public void TypeChecker_OfType_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<ITypeCondition<object>>();

            IEnumerable<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeMultipleTypeToFind_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<ITypeCondition<object>>();

            IEnumerable<ITypeSymbol> typeSymbols = new[] { typeof(ITypeCondition<object>), typeof(IClassVisitorBuilder) }.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Fact]
        public void TypeChecker_OfTypeWithGeneric_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<IEnumerable<string>>();

            IEnumerable<ITypeSymbol> typeSymbols = typeof(IEnumerable<string>).GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Fact]
        public void TypeChecker_OfTypeWithGeneric_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<IEnumerable<string>>();

            IEnumerable<ITypeSymbol> typeSymbols = typeof(IEnumerable<ITypeCondition<object>>).GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeWithTuples_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<(int, int)>();

            IEnumerable<ITypeSymbol> typeSymbols = typeof((int, int)).GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof((int, int)))]
        [InlineData(typeof((int, int, int)))]
        [InlineData(typeof((string, int)))]
        public void TypeChecker_OfTypeWithTuples_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<(int, string)>();

            IEnumerable<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeOneOfWithoutParameters_TypeCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeOneOfWithoutParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf();

            IEnumerable<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeOneOfWithNullParameters_TypeCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(null);

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeOneOfWithNullParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(null);

            IEnumerable<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(ITypeCondition<object>))]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeOneOf_ReturnsTrue(Type type)
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(typeof(ITypeCondition<object>), typeof(TypeConditionBuilderTests), typeof(IEnumerable<string>));

            IEnumerable<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<ITypeCondition<object>>))]
        [InlineData(typeof(ITypeParameterSymbol))]
        public void TypeChecker_OfTypeOneOf_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(typeof(ITypeCondition<object>), typeof(TypeConditionBuilderTests), typeof(IEnumerable<string>));

            IEnumerable<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeAllOfWithoutParameters_TypeCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeAllOfWithoutParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf();

            IEnumerable<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeAllOfWithNullParameters_TypeCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(null);

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeAllOfWithNullParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(null);

            IEnumerable<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeAllOf_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(typeof(ITypeCondition<object>), typeof(TypeConditionBuilderTests), typeof(IEnumerable<string>));

            IEnumerable<ITypeSymbol> typeSymbols = new[]
            {
                typeof(IEnumerable<string>),
                typeof(ITypeCondition<object>),
                typeof(TypeConditionBuilderTests)
            }
            .GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Fact]
        public void TypeChecker_OfTypeAllOfWithMoreTypes_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(typeof(ITypeCondition<object>), typeof(TypeConditionBuilderTests), typeof(IEnumerable<string>));

            IEnumerable<ITypeSymbol> typeSymbols = new[]
            {
                typeof(IEnumerable<string>),
                typeof(IEnumerable<DateTime>),
                typeof(ITypeCondition<object>),
                typeof(TypeConditionBuilderTests)
            }
            .GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(ITypeCondition<object>))]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeAllOf_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<IEnumerable<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<IEnumerable<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(typeof(ITypeCondition<object>), typeof(TypeConditionBuilderTests), typeof(IEnumerable<string>));

            IEnumerable<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }
    }
}