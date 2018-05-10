using System.Linq;
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
                .WithName().EqualTo("ClassNamed")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(new[] { new ClassEntity { Name = "ClassNamed", FullName = "NameInProgress.Tests.Data.ClassNamed" } });
        }

        [Fact]
        public void ClassVisitor_WithNameLike()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithName().Like("Named")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassNamed", FullName = "NameInProgress.Tests.Data.ClassNamed" },
                 new ClassEntity { Name = "ClassNamedLikeNamed", FullName = "NameInProgress.Tests.Data.ClassNamedLikeNamed" },
            };

            classes.Should().HaveCount(2);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithNameLikeIgnoringCase()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithName().Like("Named", true)
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassNamed", FullName = "NameInProgress.Tests.Data.ClassNamed" },
                 new ClassEntity { Name = "ClassNamedLikeNamed", FullName = "NameInProgress.Tests.Data.ClassNamedLikeNamed" },
                 new ClassEntity { Name = "ClassNaMEdWithWeirdCase", FullName = "NameInProgress.Tests.Data.ClassNaMEdWithWeirdCase" },
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
        public void ClassVisitor_WithAccessibilityOneOf()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithAccessibility().OneOf(MemberAccessibility.Public)
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = typeof(Struct)
                .Assembly
                .GetTypes()
                .Where(t => t.IsClass && t.IsPublic)
                .Select(t => new ClassEntity { Name = t.Name.Split('`')[0], FullName = t.GetFormattedString() });
            classes.Should().HaveCount(expected.Count());
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

        [Fact]
        public void ClassVisitor_WithNamespaceEqualTo()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithNamespace().EqualTo("NameInProgress.Tests.Data.Namespace")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(new[] { new ClassEntity { Name = "ClassWithNamespace", FullName = "NameInProgress.Tests.Data.Namespace.ClassWithNamespace" } });
        }

        [Fact]
        public void ClassVisitor_WithNamespaceLike()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithNamespace().Like("Namespace")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassWithNamespace", FullName = "NameInProgress.Tests.Data.Namespace.ClassWithNamespace" },
                 new ClassEntity { Name = "ClassWithNamespaceLikeNamespace", FullName = "NameInProgress.Tests.Data.Namespace.Like.ClassWithNamespaceLikeNamespace" },
            };

            classes.Should().HaveCount(2);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithNamespaceLikeIgnoringCase()
        {
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithNamespace().Like("Namespace", true)
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassWithNamespace", FullName = "NameInProgress.Tests.Data.Namespace.ClassWithNamespace" },
                 new ClassEntity { Name = "ClassWithNamespaceLikeNamespace", FullName = "NameInProgress.Tests.Data.Namespace.Like.ClassWithNamespaceLikeNamespace" },
                 new ClassEntity { Name = "ClassWithNamespaceWithWeirdCase", FullName = "NameInProgress.Tests.Data.NAmeSPace.ClassWithNamespaceWithWeirdCase" },
            };

            classes.Should().HaveCount(3);
            classes.Should().BeEquivalentTo(expected);
        }
    }
}