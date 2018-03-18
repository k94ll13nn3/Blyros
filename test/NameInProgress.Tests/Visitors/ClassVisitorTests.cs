using FluentAssertions;
using Xunit;

namespace NameInProgress.Tests.Visitors
{
    public class ClassVisitorTests
    {
        [Fact]
        public void Test()
        {
            'a'.Should().Be('a');
        }
    }
}