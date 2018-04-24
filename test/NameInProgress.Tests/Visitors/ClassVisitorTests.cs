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
                .WithName().EqualTo("Truc2")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(new[] { new ClassEntity { Name = "Truc2", FullName = "NameInProgress.Tests.Data.Truc2<V>" } });
        }

        [Fact]
        public void ClassVisitor_WithNameLike()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithName().Like("Truc")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "Truc", FullName = "NameInProgress.Tests.Data.Truc<U>" },
                 new ClassEntity { Name = "Truc2", FullName = "NameInProgress.Tests.Data.Truc2<V>" },
                 new ClassEntity { Name = "Truc3", FullName = "NameInProgress.Tests.Data.Truc3<V>" },
                 new ClassEntity { Name = "Truc4", FullName = "NameInProgress.Tests.Data.Truc4<V>" },
            };

            classes.Should().HaveCount(4);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithNameLikeIgnoringCase()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithName().Like("Truc", true)
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "Truc", FullName = "NameInProgress.Tests.Data.Truc<U>" },
                 new ClassEntity { Name = "Truc2", FullName = "NameInProgress.Tests.Data.Truc2<V>" },
                 new ClassEntity { Name = "Truc3", FullName = "NameInProgress.Tests.Data.Truc3<V>" },
                 new ClassEntity { Name = "Truc4", FullName = "NameInProgress.Tests.Data.Truc4<V>" },
                 new ClassEntity { Name = "TRuc", FullName = "NameInProgress.Tests.Data.TRuc" },
            };

            classes.Should().HaveCount(5);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithAccessibilityPrivate()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithAccessibility().EqualTo(MemberAccessibility.Private)
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "PrivateThing", FullName = "NameInProgress.Tests.Data.Truc<U>.PrivateThing" },
            };

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(expected);
        }
    }
}