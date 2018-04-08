using FakeItEasy;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using NameInProgress.Visitors;
using Xunit;

namespace NameInProgress.Tests.Visitors
{
    public class BaseVisitorTests
    {
        [Fact]
        public void BaseVisitor_Execute_CallsVisit()
        {
            var visitor = A.Fake<BaseVisitor<string>>();

            visitor.Execute(typeof(BaseVisitorTests));

            A.CallTo(() => visitor.Visit(A<ISymbol>._)).MustHaveHappened();
        }

        [Fact]
        public void BaseVisitor_Execute_ReturnsGetResults()
        {
            var testData = new[] { "test1", "test2", "test3", "test4" };

            var visitor = A.Fake<BaseVisitor<string>>();
            A.CallTo(() => visitor.GetResults()).Returns(testData);

            visitor.Execute(typeof(BaseVisitorTests)).Should().BeEquivalentTo(testData);
        }
    }
}