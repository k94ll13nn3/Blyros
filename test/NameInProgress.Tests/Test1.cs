using FluentAssertions;
using Xunit;

namespace NameInProgress.Tests
{
    public class Test1
    {
        [Fact]
        public void Test()
        {
            'a'.Should().Be('a');
        }
    }
}