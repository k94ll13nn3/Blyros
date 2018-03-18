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
        [Fact]
        public void AccessibilityChecker_EqualTo_ReturnsTrue()
        {
            IAccessibilityCondition builder =
                new Builder(A.Fake<IAccessibilityCondition>())
                .EqualTo(MemberAccessibility.Internal);

            builder.AccessibilityChecker(Accessibility.Internal).Should().BeTrue();
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