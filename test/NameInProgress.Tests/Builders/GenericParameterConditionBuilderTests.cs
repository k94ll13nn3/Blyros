using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using NameInProgress.Builders;
using NameInProgress.Conditions;
using NameInProgress.Enums;
using Xunit;

namespace NameInProgress.Tests.Builders
{
    public class GenericParameterConditionBuilderTests
    {
        [Fact]
        public void GenericParameterChecker_AnyType_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .AnyType();

            fakeBuilder.Checker(A.Fake<ITypeParameterSymbol>()).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfType_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<IGenericParameterCondition<object>>();

            ITypeParameterSymbol typeParameterSymbol = typeof(IGenericParameterCondition<object>).GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(IGenericParameterCondition<IGenericParameterCondition<object>>))]
        [InlineData(typeof(ITypeSymbol))]
        public void GenericParameterChecker_OfType_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<IGenericParameterCondition<object>>();

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeMultipleTypeToFind_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<IGenericParameterCondition<object>>();

            ITypeParameterSymbol typeParameterSymbol = new[] { typeof(IGenericParameterCondition<object>), typeof(IClassVisitorBuilder) }.GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithGeneric_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<IEnumerable<string>>();

            ITypeParameterSymbol typeParameterSymbol = typeof(IEnumerable<string>).GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithGeneric_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<IEnumerable<string>>();

            ITypeParameterSymbol typeParameterSymbol = typeof(IEnumerable<IGenericParameterCondition<object>>).GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithTuples_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<(int, int)>();

            ITypeParameterSymbol typeParameterSymbol = typeof((int, int)).GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof((int, int)))]
        [InlineData(typeof((int, int, int)))]
        [InlineData(typeof((string, int)))]
        public void GenericParameterChecker_OfTypeWithTuples_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType<(int, string)>();

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeOneOfWithoutParameters_GenericParameterCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeOneOfWithoutParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf();

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeOneOfWithNullParameters_GenericParameterCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(null);

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeOneOfWithNullParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(null);

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(IGenericParameterCondition<object>))]
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeOneOf_ReturnsTrue(Type type)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(typeof(IGenericParameterCondition<object>), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<IGenericParameterCondition<object>>))]
        [InlineData(typeof(ITypeParameterSymbol))]
        public void GenericParameterChecker_OfTypeOneOf_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .OneOf(typeof(IGenericParameterCondition<object>), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeAllOfWithoutParameters_GenericParameterCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeAllOfWithoutParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf();

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeAllOfWithNullParameters_GenericParameterCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(null);

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeAllOfWithNullParameters_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(null);

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeAllOf_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(typeof(IGenericParameterCondition<object>), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            ITypeParameterSymbol typeParameterSymbol = new[]
            {
                typeof(IEnumerable<string>),
                typeof(IGenericParameterCondition<object>),
                typeof(GenericParameterConditionBuilderTests)
            }
            .GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeAllOfWithMoreTypes_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(typeof(IGenericParameterCondition<object>), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            ITypeParameterSymbol typeParameterSymbol = new[]
            {
                typeof(IEnumerable<string>),
                typeof(IEnumerable<DateTime>),
                typeof(IGenericParameterCondition<object>),
                typeof(GenericParameterConditionBuilderTests)
            }
            .GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(IGenericParameterCondition<object>))]
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeAllOf_ReturnsFalse(Type type)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OfType()
                .AllOf(typeof(IGenericParameterCondition<object>), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Theory]
        [InlineData(GenericConstraint.Class, false, true, false)]
        [InlineData(GenericConstraint.New, true, false, false)]
        [InlineData(GenericConstraint.Struct, false, false, true)]
        public void GenericParameterChecker_WithConstraintEqualTo_ReturnsTrue(GenericConstraint constraint, bool hasNewConstraint, bool hasClassConstraint, bool hasStructTypeConstraint)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .EqualTo(constraint)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(hasNewConstraint);
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(hasClassConstraint);
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(hasStructTypeConstraint);

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(GenericConstraint.Class)]
        [InlineData(GenericConstraint.New)]
        [InlineData(GenericConstraint.Struct)]
        public void GenericParameterChecker_WithConstraintEqualTo_ReturnsFalse(GenericConstraint constraint)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .EqualTo(constraint)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintEqualToWithInvalidParameter_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .EqualTo((GenericConstraint)(-1))
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOfWithoutParameters_GenericParameterCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .AllOf()
                .AnyType();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOfWithoutParameters_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .AllOf()
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOfWithNullParameters_GenericParameterCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .AllOf(null)
                .AnyType();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOfWithNullParameters_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .AllOf(null)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOf_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .AllOf(GenericConstraint.New, GenericConstraint.Class)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(true);
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(true);
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(false);

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOfWithMoreConstraints_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .AllOf(GenericConstraint.New, GenericConstraint.Class)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(true);
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(true);
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(true);

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, false, false)]
        public void GenericParameterChecker_WithConstraintAllOf_ReturnsFalse(bool hasNewConstraint, bool hasClassConstraint, bool hasStructTypeConstraint)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .AllOf(GenericConstraint.New, GenericConstraint.Class)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(hasNewConstraint);
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(hasClassConstraint);
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(hasStructTypeConstraint);

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintOneOfWithoutParameters_GenericParameterCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .OneOf()
                .AnyType();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintOneOfWithoutParameters_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .OneOf()
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintOneOfWithNullParameters_GenericParameterCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .OneOf(null)
                .AnyType();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintOneOfWithNullParameters_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .OneOf(null)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, true, false)]
        public void GenericParameterChecker_WithConstraintOneOf_ReturnsTrue(bool hasNewConstraint, bool hasClassConstraint, bool hasStructTypeConstraint)
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .OneOf(GenericConstraint.New, GenericConstraint.Class)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(hasNewConstraint);
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(hasClassConstraint);
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(hasStructTypeConstraint);

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintOneOf_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .OneOf(GenericConstraint.New, GenericConstraint.Class)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(true);

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithConstraintEqualTo_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .EqualTo(GenericConstraint.Struct)
                .OfType<IEnumerable<string>>();

            ITypeParameterSymbol typeParameterSymbol = typeof(IEnumerable<string>).GetFakeTypeParameterSymbol();
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(true);

            fakeBuilder.Checker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeOneOfWithConstraintEqualToWithRightTypeButWrongConstraint_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .EqualTo(GenericConstraint.Struct)
                .OfType()
                .OneOf(typeof(IEnumerable<string>), typeof(IEnumerable<int>));

            ITypeParameterSymbol typeParameterSymbol = typeof(IEnumerable<string>).GetFakeTypeParameterSymbol();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(true);

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeOneOfWithConstraintEqualToWithWrongTypeButRightConstraint_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<ITypeParameterSymbol>();
            (new GenericParameterConditionBuilder<FakeBuilder<ITypeParameterSymbol>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .WithConstraint()
                .EqualTo(GenericConstraint.Struct)
                .OfType()
                .OneOf(typeof(IEnumerable<string>), typeof(IEnumerable<int>));

            ITypeParameterSymbol typeParameterSymbol = typeof(IEnumerable<DateTime>).GetFakeTypeParameterSymbol();
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(true);

            fakeBuilder.Checker(typeParameterSymbol).Should().BeFalse();
        }
    }
}