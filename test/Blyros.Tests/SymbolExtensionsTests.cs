using Blyros.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Blyros.Tests
{
    public class SymbolExtensionsTests
    {
        [Fact]
        public void IsSpecial_SpecialName_ReturnsTrue()
        {
            ISymbol symbol = A.Fake<ISymbol>();
            A.CallTo(() => symbol.ToString()).Returns("<Module>");

            bool result = symbol.IsSpecial();

            result.Should().BeTrue();
        }

        [Fact]
        public void IsSpecial_NormalName_ReturnsFalse()
        {
            ISymbol symbol = A.Fake<ISymbol>();
            A.CallTo(() => symbol.ToString()).Returns("Module");

            bool result = symbol.IsSpecial();

            result.Should().BeFalse();
        }

        [Fact]
        public void IsSpecial_NormalNameWithStartingAngleBracket_ReturnsFalse()
        {
            ISymbol symbol = A.Fake<ISymbol>();
            A.CallTo(() => symbol.ToString()).Returns("<Module");

            bool result = symbol.IsSpecial();

            result.Should().BeFalse();
        }

        [Fact]
        public void IsSpecial_NormalNameWithEndingAngleBracket_ReturnsFalse()
        {
            ISymbol symbol = A.Fake<ISymbol>();
            A.CallTo(() => symbol.ToString()).Returns("Module>");

            bool result = symbol.IsSpecial();

            result.Should().BeFalse();
        }
    }
}
