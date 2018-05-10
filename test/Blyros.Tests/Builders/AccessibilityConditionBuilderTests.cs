using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Blyros.Builders;
using Blyros.Conditions;
using Blyros.Enums;
using Xunit;

namespace Blyros.Tests.Builders
{
    public class AccessibilityConditionBuilderTests
    {
        [Theory]
        [InlineData(MemberAccessibility.Internal, Accessibility.Internal)]
        [InlineData(MemberAccessibility.Private, Accessibility.Private)]
        [InlineData(MemberAccessibility.PrivateProtected, Accessibility.ProtectedAndInternal)]
        [InlineData(MemberAccessibility.Protected, Accessibility.Protected)]
        [InlineData(MemberAccessibility.ProtectedInternal, Accessibility.ProtectedOrInternal)]
        [InlineData(MemberAccessibility.Public, Accessibility.Public)]
        public void AccessibilityChecker_EqualTo_ReturnsTrue(MemberAccessibility memberAccessibility, Accessibility accessibility)
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .EqualTo(memberAccessibility);

            fakeBuilder.Checker(accessibility).Should().BeTrue();
        }

        [Fact]
        public void AccessibilityChecker_EqualTo_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .EqualTo(MemberAccessibility.Internal);

            fakeBuilder.Checker(Accessibility.Protected).Should().BeFalse();
        }

        [Fact]
        public void AccessibilityChecker_EqualToWithInvalidParameter_ThrowsArgumentException()
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            Action act = () => (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .EqualTo((MemberAccessibility)(-1));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AccessibilityChecker_OneOf_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf(MemberAccessibility.Internal, MemberAccessibility.Public);

            fakeBuilder.Checker(Accessibility.Protected).Should().BeFalse();
            fakeBuilder.Checker(Accessibility.Private).Should().BeFalse();
        }

        [Fact]
        public void AccessibilityChecker_OneOf_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf(MemberAccessibility.Internal, MemberAccessibility.Public);

            fakeBuilder.Checker(Accessibility.Internal).Should().BeTrue();
            fakeBuilder.Checker(Accessibility.Public).Should().BeTrue();
        }

        [Fact]
        public void AccessibilityChecker_OneOfWithoutParameters_AccessibilityCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(Accessibility.Internal)]
        [InlineData(Accessibility.Private)]
        [InlineData(Accessibility.ProtectedAndInternal)]
        [InlineData(Accessibility.Protected)]
        [InlineData(Accessibility.ProtectedOrInternal)]
        [InlineData(Accessibility.Public)]
        public void AccessibilityChecker_OneOfWithoutParameters_ReturnsFalse(Accessibility accessibility)
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf();

            fakeBuilder.Checker(accessibility).Should().BeFalse();
        }

        [Fact]
        public void AccessibilityChecker_OneOfWithNullParameters_AccessibilityCheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf(null);

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(Accessibility.Internal)]
        [InlineData(Accessibility.Private)]
        [InlineData(Accessibility.ProtectedAndInternal)]
        [InlineData(Accessibility.Protected)]
        [InlineData(Accessibility.ProtectedOrInternal)]
        [InlineData(Accessibility.Public)]
        public void AccessibilityChecker_OneOfWithNullParameters_ReturnsFalse(Accessibility accessibility)
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf(null);

            fakeBuilder.Checker(accessibility).Should().BeFalse();
        }

        [Theory]
        [InlineData(Accessibility.Internal)]
        [InlineData(Accessibility.Private)]
        [InlineData(Accessibility.ProtectedAndInternal)]
        [InlineData(Accessibility.Protected)]
        [InlineData(Accessibility.ProtectedOrInternal)]
        [InlineData(Accessibility.Public)]
        public void AccessibilityChecker_OneOfWithAllParameters_ReturnsTrue(Accessibility accessibility)
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf(Enum.GetValues(typeof(MemberAccessibility)).Cast<MemberAccessibility>().ToArray());

            fakeBuilder.Checker(accessibility).Should().BeTrue();
        }

        [Fact]
        public void AccessibilityChecker_OneOfWithInvalidParameter_ThrowsArgumentException()
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            Action act = () => (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf((MemberAccessibility)(-1));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AccessibilityChecker_EqualToWithComposedAccessibility_ReturnsFalse()
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .EqualTo(MemberAccessibility.ProtectedInternal);

            fakeBuilder.Checker(Accessibility.Internal).Should().BeFalse();
            fakeBuilder.Checker(Accessibility.Protected).Should().BeFalse();
        }

        [Fact]
        public void AccessibilityChecker_EqualToWithComposedAccessibility_ReturnsTrue()
        {
            var fakeBuilder = new FakeBuilder<Accessibility>();
            (new AccessibilityConditionBuilder<FakeBuilder<Accessibility>>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .EqualTo(MemberAccessibility.PrivateProtected);

            fakeBuilder.Checker(Accessibility.ProtectedAndInternal).Should().BeTrue();
        }
    }
}