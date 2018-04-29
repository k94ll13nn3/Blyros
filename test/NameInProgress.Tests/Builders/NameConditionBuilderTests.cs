using FluentAssertions;
using NameInProgress.Builders;
using Xunit;

namespace NameInProgress.Tests.Builders
{
    public class StringConditionBuilderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(nameof(StringConditionBuilderTests))]
        public void Checker_EqualTo_ReturnsTrue(string name)
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .EqualTo(name);

            fakeBuilder.Checker(name).Should().BeTrue();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "Lorem")]
        [InlineData(nameof(StringConditionBuilderTests), nameof(FakeBuilder))]
        public void Checker_EqualTo_ReturnsFalse(string name, string equalTo)
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .EqualTo(equalTo);

            fakeBuilder.Checker(name).Should().BeFalse();
        }

        [Theory]
        [InlineData("Lorem", "Lor")]
        [InlineData("Test", "es")]
        [InlineData("Lorem", "")]
        public void Checker_Like_ReturnsTrue(string name, string pattern)
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .Like(pattern);

            fakeBuilder.Checker(name).Should().BeTrue();
        }

        [Theory]
        [InlineData("Lorem", "lor")]
        [InlineData("Lorem", "LoremLorem")]
        [InlineData("Lorem", null)]
        [InlineData(null, "")]
        public void Checker_Like_ReturnsFalse(string name, string pattern)
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .Like(pattern);

            fakeBuilder.Checker(name).Should().BeFalse();
        }

        [Theory]
        [InlineData("Lorem", "lor")]
        [InlineData("Lorem", "")]
        [InlineData("lorem", "LOREM")]
        public void Checker_LikeIgnoringCase_ReturnsTrue(string name, string pattern)
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .Like(pattern, true);

            fakeBuilder.Checker(name).Should().BeTrue();
        }

        [Fact]
        public void Checker_OneOfWithoutParameters_CheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf();

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(nameof(StringConditionBuilderTests))]
        public void Checker_OneOfWithoutParameters_ReturnsFalse(string name)
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf();

            fakeBuilder.Checker(name).Should().BeFalse();
        }

        [Fact]
        public void Checker_OneOfWithNullParameters_CheckerShouldNotBeNull()
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf(null);

            fakeBuilder.Checker.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(nameof(StringConditionBuilderTests))]
        public void Checker_OneOfWithNullParameters_ReturnsFalse(string name)
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf(null);

            fakeBuilder.Checker(name).Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData("Lorem")]
        [InlineData(nameof(StringConditionBuilderTests))]
        public void Checker_OneOf_ReturnsTrue(string name)
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf(new[] { "", "Lorem", nameof(StringConditionBuilderTests) });

            fakeBuilder.Checker(name).Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(nameof(FakeBuilder))]
        [InlineData("LoremLorem")]
        public void Checker_OneOf_ReturnsFalse(string name)
        {
            var fakeBuilder = new FakeBuilder();
            (new StringConditionBuilder<FakeBuilder>(fakeBuilder, _ => fakeBuilder.SetChecker(_)))
                .OneOf(new[] { "", "Lorem", nameof(StringConditionBuilderTests) });

            fakeBuilder.Checker(name).Should().BeFalse();
        }
    }
}