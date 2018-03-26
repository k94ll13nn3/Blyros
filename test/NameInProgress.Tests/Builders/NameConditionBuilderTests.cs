using FakeItEasy;
using FluentAssertions;
using NameInProgress.Builders;
using Xunit;
using Builder = NameInProgress.Builders.NameConditionBuilder<NameInProgress.Builders.INameCondition, NameInProgress.Builders.INameCondition>;

namespace NameInProgress.Tests.Builders
{
    public class NameConditionBuilderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(nameof(NameConditionBuilderTests))]
        public void NameChecker_EqualTo_ReturnsTrue(string name)
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .EqualTo(name);

            builder.NameChecker(name).Should().BeTrue();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "Lorem")]
        [InlineData(nameof(NameConditionBuilderTests), nameof(INameCondition))]
        public void NameChecker_EqualTo_ReturnsFalse(string name, string equalTo)
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .EqualTo(equalTo);

            builder.NameChecker(name).Should().BeFalse();
        }

        [Theory]
        [InlineData("Lorem", "Lor")]
        [InlineData("Test", "es")]
        [InlineData("Lorem", "")]
        public void NameChecker_Like_ReturnsTrue(string name, string pattern)
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .Like(pattern);

            builder.NameChecker(name).Should().BeTrue();
        }

        [Theory]
        [InlineData("Lorem", "lor")]
        [InlineData("Lorem", "LoremLorem")]
        [InlineData("Lorem", null)]
        [InlineData(null, "")]
        public void NameChecker_Like_ReturnsFalse(string name, string pattern)
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .Like(pattern);

            builder.NameChecker(name).Should().BeFalse();
        }

        [Theory]
        [InlineData("Lorem", "lor")]
        [InlineData("Lorem", "")]
        [InlineData("lorem", "LOREM")]
        public void NameChecker_LikeIgnoringCase_ReturnsTrue(string name, string pattern)
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .Like(pattern, true);

            builder.NameChecker(name).Should().BeTrue();
        }

        [Fact]
        public void NameChecker_OneOfWithoutParameters_NameCheckerShouldNotBeNull()
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .OneOf();

            builder.NameChecker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(nameof(NameConditionBuilderTests))]
        public void NameChecker_OneOfWithoutParameters_ReturnsFalse(string name)
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .OneOf();

            builder.NameChecker(name).Should().BeFalse();
        }

        [Fact]
        public void NameChecker_OneOfWithNullParameters_NameCheckerShouldNotBeNull()
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .OneOf(null);

            builder.NameChecker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(nameof(NameConditionBuilderTests))]
        public void NameChecker_OneOfWithNullParameters_ReturnsFalse(string name)
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .OneOf(null);

            builder.NameChecker(name).Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Lorem")]
        [InlineData(nameof(NameConditionBuilderTests))]
        public void NameChecker_OneOf_ReturnsTrue(string name)
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .OneOf(new[] { "", "Lorem", nameof(NameConditionBuilderTests) });

            builder.NameChecker(name).Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(nameof(INameCondition))]
        [InlineData("LoremLorem")]
        public void NameChecker_OneOf_ReturnsFalse(string name)
        {
            INameCondition builder =
                new Builder(A.Fake<INameCondition>())
                .OneOf(new[] { "", "Lorem", nameof(NameConditionBuilderTests) });

            builder.NameChecker(name).Should().BeFalse();
        }
    }
}