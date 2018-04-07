using System.Collections.Generic;
using System.Collections.Immutable;
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

        [Fact]
        public void GenericParameterChecker_OfTypeAllOfWithMoreTypes_ReturnsTrue()
        {
            IGenericParameterChecker builder =
                (new Builder(A.Fake<IGenericParameterChecker>()) as IGenericParameterCondition<IGenericParameterChecker>)
                .OfType()
                .AllOf(typeof(IGenericParameterChecker), typeof(GenericParameterConditionBuilderTests), typeof(IEnumerable<string>));

            var typeParameterSymbol = GetTypeParameterSymbol(
                "System.Collections.Generic.IEnumerable<System.String>",
                "System.Collections.Generic.IEnumerable<System.DateTime>",
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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();

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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
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

            var typeParameterSymbol = A.Fake<ITypeParameterSymbol>();
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

            var typeParameterSymbol = GetTypeParameterSymbol("System.Collections.Generic.IEnumerable<System.String>");
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

            var typeParameterSymbol = GetTypeParameterSymbol("System.Collections.Generic.IEnumerable<System.String>");
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

            var typeParameterSymbol = GetTypeParameterSymbol("System.Collections.Generic.IEnumerable<System.DateTime>");
            A.CallTo(() => typeParameterSymbol.HasReferenceTypeConstraint).Returns(true);

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