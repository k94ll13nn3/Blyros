using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using NameInProgress.Builders;
using NameInProgress.Conditions;
using NameInProgress.Enums;
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

            ITypeParameterSymbol typeParameterSymbol = typeof(IGenericParameterChecker).GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(IGenericParameterCondition<IGenericParameterChecker>))]
        [InlineData(typeof(ITypeSymbol))]
        public void GenericParameterChecker_OfType_ReturnsFalse(Type type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<IGenericParameterChecker>();

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeMultipleTypeToFind_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<IGenericParameterChecker>();

            ITypeParameterSymbol typeParameterSymbol = new[] { typeof(IGenericParameterChecker), typeof(IAccessibilityChecker) }.GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithGeneric_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<IEnumerable<string>>();

            ITypeParameterSymbol typeParameterSymbol = typeof(IEnumerable<string>).GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithGeneric_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<IEnumerable<string>>();

            ITypeParameterSymbol typeParameterSymbol = typeof(IEnumerable<IGenericParameterChecker>).GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithTuples_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<(int, int)>();

            ITypeParameterSymbol typeParameterSymbol = typeof((int, int)).GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof((int, int)))]
        [InlineData(typeof((int, int, int)))]
        [InlineData(typeof((string, int)))]
        public void GenericParameterChecker_OfTypeWithTuples_ReturnsFalse(Type type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType<(int, string)>();

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

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
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeOneOfWithoutParameters_ReturnsFalse(Type type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .OneOf();

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

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
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeOneOfWithNullParameters_ReturnsFalse(Type type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .OneOf(null);

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(IGenericParameterChecker))]
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeOneOf_ReturnsTrue(Type type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .OneOf(typeof(IGenericParameterChecker), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<IGenericParameterChecker>))]
        [InlineData(typeof(ITypeParameterSymbol))]
        public void GenericParameterChecker_OfTypeOneOf_ReturnsFalse(Type type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .OneOf(typeof(IGenericParameterChecker), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

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
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeAllOfWithoutParameters_ReturnsFalse(Type type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf();

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

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
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeAllOfWithNullParameters_ReturnsFalse(Type type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf(null);

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeAllOf_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf(typeof(IGenericParameterChecker), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            ITypeParameterSymbol typeParameterSymbol = new[]
            {
                typeof(IEnumerable<string>),
                typeof(IGenericParameterChecker),
                typeof(GenericParameterConditionBuilderTests)
            }
            .GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeAllOfWithMoreTypes_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf(typeof(IGenericParameterChecker), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            ITypeParameterSymbol typeParameterSymbol = new[]
            {
                typeof(IEnumerable<string>),
                typeof(IEnumerable<DateTime>),
                typeof(IGenericParameterChecker),
                typeof(GenericParameterConditionBuilderTests)
            }
            .GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(typeof(IEnumerable<string>))]
        [InlineData(typeof(IGenericParameterChecker))]
        [InlineData(typeof(GenericParameterConditionBuilderTests))]
        public void GenericParameterChecker_OfTypeAllOf_ReturnsFalse(Type type)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf(typeof(IGenericParameterChecker), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            ITypeParameterSymbol typeParameterSymbol = type.GetFakeTypeParameterSymbol();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Theory]
        [InlineData(GenericConstraint.Class, false, true, false)]
        [InlineData(GenericConstraint.New, true, false, false)]
        [InlineData(GenericConstraint.Struct, false, false, true)]
        public void GenericParameterChecker_WithConstraintEqualTo_ReturnsTrue(GenericConstraint constraint, bool hasNewConstraint, bool hasClassConstraint, bool hasStructTypeConstraint)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .EqualTo(constraint)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(hasNewConstraint);
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(hasClassConstraint);
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(hasStructTypeConstraint);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(GenericConstraint.Class)]
        [InlineData(GenericConstraint.New)]
        [InlineData(GenericConstraint.Struct)]
        public void GenericParameterChecker_WithConstraintEqualTo_ReturnsFalse(GenericConstraint constraint)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .EqualTo(constraint)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintEqualToWithInvalidParameter_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .EqualTo((GenericConstraint)(-1))
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOfWithoutParameters_GenericParameterCheckerShouldNotBeNull()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .AllOf()
                .AnyType();

            builder.GenericParameterChecker.Should().NotBeNull();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOfWithoutParameters_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .AllOf()
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOfWithNullParameters_GenericParameterCheckerShouldNotBeNull()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .AllOf(null)
                .AnyType();

            builder.GenericParameterChecker.Should().NotBeNull();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOfWithNullParameters_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .AllOf(null)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOf_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .AllOf(GenericConstraint.New, GenericConstraint.Class)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(true);
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(true);
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(false);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintAllOfWithMoreConstraints_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .AllOf(GenericConstraint.New, GenericConstraint.Class)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(true);
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(true);
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(true);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, false, false)]
        public void GenericParameterChecker_WithConstraintAllOf_ReturnsFalse(bool hasNewConstraint, bool hasClassConstraint, bool hasStructTypeConstraint)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .AllOf(GenericConstraint.New, GenericConstraint.Class)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(hasNewConstraint);
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(hasClassConstraint);
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(hasStructTypeConstraint);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintOneOfWithoutParameters_GenericParameterCheckerShouldNotBeNull()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .OneOf()
                .AnyType();

            builder.GenericParameterChecker.Should().NotBeNull();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintOneOfWithoutParameters_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .OneOf()
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintOneOfWithNullParameters_GenericParameterCheckerShouldNotBeNull()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .OneOf(null)
                .AnyType();

            builder.GenericParameterChecker.Should().NotBeNull();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintOneOfWithNullParameters_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .OneOf(null)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, true, false)]
        public void GenericParameterChecker_WithConstraintOneOf_ReturnsTrue(bool hasNewConstraint, bool hasClassConstraint, bool hasStructTypeConstraint)
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .OneOf(GenericConstraint.New, GenericConstraint.Class)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(hasNewConstraint);
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(hasClassConstraint);
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(hasStructTypeConstraint);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_WithConstraintOneOf_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .OneOf(GenericConstraint.New, GenericConstraint.Class)
                .AnyType();

            ITypeParameterSymbol typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(true);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeWithConstraintEqualTo_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .EqualTo(GenericConstraint.Struct)
                .OfType<IEnumerable<string>>();

            ITypeParameterSymbol typeParameterSymbol = typeof(IEnumerable<string>).GetFakeTypeParameterSymbol();
            A.CallTo(() => typeParameterSymbol.HasValueTypeConstraint).Returns(true);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeTrue();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeOneOfWithConstraintEqualToWithRightTypeButWrongConstraint_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .EqualTo(GenericConstraint.Struct)
                .OfType()
                .OneOf(typeof(IEnumerable<string>), typeof(IEnumerable<int>));

            ITypeParameterSymbol typeParameterSymbol = typeof(IEnumerable<string>).GetFakeTypeParameterSymbol();
            A.CallTo(() => typeParameterSymbol.HasConstructorConstraint).Returns(true);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }

        [Fact]
        public void GenericParameterChecker_OfTypeOneOfWithConstraintEqualToWithWrongTypeButRightConstraint_ReturnsFalse()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .WithConstraint()
                .EqualTo(GenericConstraint.Struct)
                .OfType()
                .OneOf(typeof(IEnumerable<string>), typeof(IEnumerable<int>));

            ITypeParameterSymbol typeParameterSymbol = typeof(IEnumerable<DateTime>).GetFakeTypeParameterSymbol();
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(true);

            builder.GenericParameterChecker(typeParameterSymbol).Should().BeFalse();
        }
    }
}