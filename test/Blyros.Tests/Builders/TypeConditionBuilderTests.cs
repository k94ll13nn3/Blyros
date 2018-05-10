using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Blyros.Builders;
using Blyros.Conditions;
using Xunit;

namespace Blyros.Tests.Builders
{
    public class TypeConditionBuilderTests
    {
        [Fact]
        public void TypeChecker_AnyType_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .AnyType();

            fakeBuilder.Checker(ImmutableArray.Create<ITypeSymbol>()).Should().BeTrue();
        }

        [Fact]
        public void TypeChecker_OfType_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<ITypeCondition<object>>();

            ImmutableArray<ITypeSymbol> typeSymbols = typeof(ITypeCondition<object>).GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(ITypeCondition<ITypeCondition<object>>))]
        [InlineData(typeof(ITypeSymbol))]
        public void TypeChecker_OfType_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<ITypeCondition<object>>();

            ImmutableArray<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeMultipleTypeToFind_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<ITypeCondition<object>>();

            ImmutableArray<ITypeSymbol> typeSymbols = new[] { typeof(ITypeCondition<object>), typeof(IClassVisitorBuilder) }.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Fact]
        public void TypeChecker_OfTypeWithGeneric_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<IEnumerable<string>>();

            ImmutableArray<ITypeSymbol> typeSymbols = typeof(IEnumerable<string>).GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Fact]
        public void TypeChecker_OfTypeWithGeneric_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<IEnumerable<string>>();

            ImmutableArray<ITypeSymbol> typeSymbols = typeof(IEnumerable<ITypeCondition<object>>).GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeWithTuples_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<(int, int)>();

            ImmutableArray<ITypeSymbol> typeSymbols = typeof((int, int)).GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof((int, int)))]
        [InlineData(typeof((int, int, int)))]
        [InlineData(typeof((string, int)))]
        public void TypeChecker_OfTypeWithTuples_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<(int, string)>();

            ImmutableArray<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeOneOfWithoutParameters_TypeCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeOneOfWithoutParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf();

            ImmutableArray<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeOneOfWithNullParameters_TypeCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(null);

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeOneOfWithNullParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(null);

            ImmutableArray<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(ITypeCondition<object>))]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeOneOf_ReturnsTrue(Type type)
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(typeof(ITypeCondition<object>), typeof(TypeConditionBuilderTests), typeof(IEnumerable<string>));

            ImmutableArray<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<ITypeCondition<object>>))]
        [InlineData(typeof(ITypeParameterSymbol))]
        public void TypeChecker_OfTypeOneOf_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(typeof(ITypeCondition<object>), typeof(TypeConditionBuilderTests), typeof(IEnumerable<string>));

            ImmutableArray<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeAllOfWithoutParameters_TypeCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeAllOfWithoutParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf();

            ImmutableArray<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeAllOfWithNullParameters_TypeCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(null);

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(TypeConditionBuilderTests))]
        public void TypeChecker_OfTypeAllOfWithNullParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(null);

            ImmutableArray<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }

        [Fact]
        public void TypeChecker_OfTypeAllOf_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(typeof(ITypeCondition<object>), typeof(TypeConditionBuilderTests), typeof(IEnumerable<string>));

            ImmutableArray<ITypeSymbol> typeSymbols = new[]
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
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(typeof(ITypeCondition<object>), typeof(TypeConditionBuilderTests), typeof(IEnumerable<string>));

            ImmutableArray<ITypeSymbol> typeSymbols = new[]
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
            var fakeBuilder = new FakeBuilder<ImmutableArray<ITypeSymbol>>();
            (new TypeConditionBuilder<FakeBuilder<ImmutableArray<ITypeSymbol>>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(typeof(ITypeCondition<object>), typeof(TypeConditionBuilderTests), typeof(IEnumerable<string>));

            ImmutableArray<ITypeSymbol> typeSymbols = type.GetFakeTypeSymbols();

            fakeBuilder.Checker(typeSymbols).Should().BeFalse();
        }
    }
}