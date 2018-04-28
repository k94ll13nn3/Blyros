using FluentAssertions;
using NameInProgress.Builders;
using NameInProgress.Entities;
using NameInProgress.Enums;
using NameInProgress.Tests.Data;
using Xunit;

namespace NameInProgress.Tests.Visitors
{
    public class ClassVisitorTests
    {
        [Fact]
        public void ClassVisitor_WithNameEqualTo()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithName().EqualTo("Name")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(new[] { new ClassEntity { Name = "Name", FullName = "NameInProgress.Tests.Data.Name" } });
        }

        [Fact]
        public void ClassVisitor_WithNameLike()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithName().Like("Name")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "Name", FullName = "NameInProgress.Tests.Data.Name" },
                 new ClassEntity { Name = "ClassWithNameLikeName", FullName = "NameInProgress.Tests.Data.ClassWithNameLikeName" },
            };

            classes.Should().HaveCount(2);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithNameLikeIgnoringCase()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithName().Like("Name", true)
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "Name", FullName = "NameInProgress.Tests.Data.Name" },
                 new ClassEntity { Name = "ClassWithNameLikeName", FullName = "NameInProgress.Tests.Data.ClassWithNameLikeName" },
                 new ClassEntity { Name = "ClassWithNAmEWithWeirdCase", FullName = "NameInProgress.Tests.Data.ClassWithNAmEWithWeirdCase" },
            };

            classes.Should().HaveCount(3);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithAccessibilityEqualTo()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithAccessibility().EqualTo(MemberAccessibility.Private)
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "PrivateClass", FullName = "NameInProgress.Tests.Data.ClassWithInnerPrivateClass.PrivateClass" },
            };

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithGenericParameterOfType()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithGenericParameter().OfType<IInterface>()
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassWithGenericParameterIInterface", FullName = "NameInProgress.Tests.Data.ClassWithGenericParameterIInterface<T>" },
            };

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithGenericParameterWithConstraintEqualToAnyType()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithGenericParameter().WithConstraint().EqualTo(GenericConstraint.New).AnyType()
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassWithGenericParameterWithNewConstraint", FullName = "NameInProgress.Tests.Data.ClassWithGenericParameterWithNewConstraint<T>" },
            };

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(expected);
        }
    }
}