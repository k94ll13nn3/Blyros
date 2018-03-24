using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using NameInProgress.Builders;
using NameInProgress.Enums;
using Xunit;
using Builder = NameInProgress.Builders.AccessibilityConditionBuilder<NameInProgress.Builders.IAccessibilityCondition, NameInProgress.Builders.IAccessibilityCondition>;

namespace NameInProgress.Tests.Builders
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
            IAccessibilityCondition builder =
                new Builder(A.Fake<IAccessibilityCondition>())
                .EqualTo(memberAccessibility);

            builder.AccessibilityChecker(accessibility).Should().BeTrue();
        }

        [Fact]
        public void AccessibilityChecker_EqualTo_ReturnsFalse()
        {
            IAccessibilityCondition builder =
                new Builder(A.Fake<IAccessibilityCondition>())
                .EqualTo(MemberAccessibility.Internal);

            builder.AccessibilityChecker(Accessibility.Protected).Should().BeFalse();
        }

        [Fact]
        public void AccessibilityChecker_OneOf_ReturnsFalse()
        {
            IAccessibilityCondition builder =
                new Builder(A.Fake<IAccessibilityCondition>())
                .OneOf(MemberAccessibility.Internal, MemberAccessibility.Public);

            builder.AccessibilityChecker(Accessibility.Protected).Should().BeFalse();
            builder.AccessibilityChecker(Accessibility.Private).Should().BeFalse();
        }

        [Fact]
        public void AccessibilityChecker_OneOf_ReturnsTrue()
        {
            IAccessibilityCondition builder =
                new Builder(A.Fake<IAccessibilityCondition>())
                .OneOf(MemberAccessibility.Internal, MemberAccessibility.Public);

            builder.AccessibilityChecker(Accessibility.Internal).Should().BeTrue();
            builder.AccessibilityChecker(Accessibility.Public).Should().BeTrue();
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
            IAccessibilityCondition builder =
                new Builder(A.Fake<IAccessibilityCondition>())
                .OneOf();

            builder.AccessibilityChecker(accessibility).Should().BeFalse();
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
            IAccessibilityCondition builder =
                new Builder(A.Fake<IAccessibilityCondition>())
                .OneOf(Enum.GetValues(typeof(MemberAccessibility)).Cast<MemberAccessibility>().ToArray());

            builder.AccessibilityChecker(accessibility).Should().BeTrue();
        }

        [Fact]
        public void AccessibilityChecker_OneOfWithInvalidParameter_ThrowsArgumentException()
        {
            Action act = () => new Builder(A.Fake<IAccessibilityCondition>()).OneOf((MemberAccessibility)(-1));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AccessibilityChecker_EqualToWithComposedAccessibility_ReturnsFalse()
        {
            IAccessibilityCondition builder =
                new Builder(A.Fake<IAccessibilityCondition>())
                .EqualTo(MemberAccessibility.ProtectedInternal);

            builder.AccessibilityChecker(Accessibility.Internal).Should().BeFalse();
            builder.AccessibilityChecker(Accessibility.Protected).Should().BeFalse();
        }

        [Fact]
        public void AccessibilityChecker_EqualToWithComposedAccessibility_ReturnsTrue()
        {
            IAccessibilityCondition builder =
                new Builder(A.Fake<IAccessibilityCondition>())
                .EqualTo(MemberAccessibility.PrivateProtected);

            builder.AccessibilityChecker(Accessibility.ProtectedAndInternal).Should().BeTrue();
        }
    }
}