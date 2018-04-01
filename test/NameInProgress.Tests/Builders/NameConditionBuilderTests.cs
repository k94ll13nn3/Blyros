using FakeItEasy;
using FluentAssertions;
using NameInProgress.Builders;
using Xunit;
using Builder = NameInProgress.Builders.NameConditionBuilder<NameInProgress.Builders.INameChecker, NameInProgress.Builders.INameChecker>;

namespace NameInProgress.Tests.Builders
{
    public class NameConditionBuilderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(nameof(NameConditionBuilderTests))]
        public void NameChecker_EqualTo_ReturnsTrue(string name)
        {
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
                .EqualTo(name);

            builder.NameChecker(name).Should().BeTrue();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "Lorem")]
        [InlineData(nameof(NameConditionBuilderTests), nameof(INameChecker))]
        public void NameChecker_EqualTo_ReturnsFalse(string name, string equalTo)
        {
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
                .EqualTo(equalTo);

            builder.NameChecker(name).Should().BeFalse();
        }

        [Theory]
        [InlineData("Lorem", "Lor")]
        [InlineData("Test", "es")]
        [InlineData("Lorem", "")]
        public void NameChecker_Like_ReturnsTrue(string name, string pattern)
        {
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
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
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
                .Like(pattern);

            builder.NameChecker(name).Should().BeFalse();
        }

        [Theory]
        [InlineData("Lorem", "lor")]
        [InlineData("Lorem", "")]
        [InlineData("lorem", "LOREM")]
        public void NameChecker_LikeIgnoringCase_ReturnsTrue(string name, string pattern)
        {
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
                .Like(pattern, true);

            builder.NameChecker(name).Should().BeTrue();
        }

        [Fact]
        public void NameChecker_OneOfWithoutParameters_NameCheckerShouldNotBeNull()
        {
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
                .OneOf();

            builder.NameChecker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(nameof(NameConditionBuilderTests))]
        public void NameChecker_OneOfWithoutParameters_ReturnsFalse(string name)
        {
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
                .OneOf();

            builder.NameChecker(name).Should().BeFalse();
        }

        [Fact]
        public void NameChecker_OneOfWithNullParameters_NameCheckerShouldNotBeNull()
        {
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
                .OneOf(null);

            builder.NameChecker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(nameof(NameConditionBuilderTests))]
        public void NameChecker_OneOfWithNullParameters_ReturnsFalse(string name)
        {
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
                .OneOf(null);

            builder.NameChecker(name).Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Lorem")]
        [InlineData(nameof(NameConditionBuilderTests))]
        public void NameChecker_OneOf_ReturnsTrue(string name)
        {
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
                .OneOf(new[] { "", "Lorem", nameof(NameConditionBuilderTests) });

            builder.NameChecker(name).Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(nameof(INameChecker))]
        [InlineData("LoremLorem")]
        public void NameChecker_OneOf_ReturnsFalse(string name)
        {
            INameChecker builder =
                new Builder(A.Fake<INameChecker>())
                .OneOf(new[] { "", "Lorem", nameof(NameConditionBuilderTests) });

            builder.NameChecker(name).Should().BeFalse();
        }
    }
}