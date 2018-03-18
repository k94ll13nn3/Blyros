using FluentAssertions;
using Xunit;

namespace NameInProgress.Tests.Builders
{
    public class AccessibilityConditionBuilderTests
    {
        [Fact]
        public void Test()
        {
            'a'.Should().Be('a');
        }
    }
}