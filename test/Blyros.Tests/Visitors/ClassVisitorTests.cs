using System;
using System.Linq;
using Blyros.Builders;
using Blyros.Entities;
using Blyros.Enums;
using Blyros.Tests.Data;
using FluentAssertions;
using Xunit;

namespace Blyros.Tests.Visitors
{
    public class ClassVisitorTests
    {
        [Fact]
        public void ClassVisitor_WithNameEqualTo()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithName().EqualTo("ClassNamed")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(new[] { new ClassEntity { Name = "ClassNamed", FullName = "Blyros.Tests.Data.ClassNamed" } });
        }

        [Fact]
        public void ClassVisitor_WithNameLike()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithName().Like("Named")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassNamed", FullName = "Blyros.Tests.Data.ClassNamed" },
                 new ClassEntity { Name = "ClassNamedLikeNamed", FullName = "Blyros.Tests.Data.ClassNamedLikeNamed" },
            };

            classes.Should().HaveCount(2);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithNameLikeIgnoringCase()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithName().Like("Named", true)
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassNamed", FullName = "Blyros.Tests.Data.ClassNamed" },
                 new ClassEntity { Name = "ClassNamedLikeNamed", FullName = "Blyros.Tests.Data.ClassNamedLikeNamed" },
                 new ClassEntity { Name = "ClassNaMEdWithWeirdCase", FullName = "Blyros.Tests.Data.ClassNaMEdWithWeirdCase" },
            };

            classes.Should().HaveCount(3);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithAccessibilityEqualTo()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithAccessibility().EqualTo(MemberAccessibility.Private)
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "PrivateClass", FullName = "Blyros.Tests.Data.ClassWithInnerPrivateClass.PrivateClass" },
            };

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithAccessibilityOneOf()
        {
            var builder = BlyrosBuilder
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
            var builder = BlyrosBuilder
                .GetClasses()
                .WithGenericParameter().OfType<IInterface>()
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassWithGenericParameterIInterface", FullName = "Blyros.Tests.Data.ClassWithGenericParameterIInterface<T>" },
            };

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithGenericParameterWithConstraintEqualToAnyType()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithGenericParameter().WithConstraint().EqualTo(GenericConstraint.New).AnyType()
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassWithGenericParameterWithNewConstraint", FullName = "Blyros.Tests.Data.ClassWithGenericParameterWithNewConstraint<T>" },
            };

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithNamespaceEqualTo()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithNamespace().EqualTo("Blyros.Tests.Data.Namespace")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(new[] { new ClassEntity { Name = "ClassWithNamespace", FullName = "Blyros.Tests.Data.Namespace.ClassWithNamespace" } });
        }

        [Fact]
        public void ClassVisitor_WithNamespaceLike()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithNamespace().Like("Namespace")
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassWithNamespace", FullName = "Blyros.Tests.Data.Namespace.ClassWithNamespace" },
                 new ClassEntity { Name = "ClassWithNamespaceLikeNamespace", FullName = "Blyros.Tests.Data.Namespace.Like.ClassWithNamespaceLikeNamespace" },
            };

            classes.Should().HaveCount(2);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithNamespaceLikeIgnoringCase()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithNamespace().Like("Namespace", true)
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassWithNamespace", FullName = "Blyros.Tests.Data.Namespace.ClassWithNamespace" },
                 new ClassEntity { Name = "ClassWithNamespaceLikeNamespace", FullName = "Blyros.Tests.Data.Namespace.Like.ClassWithNamespaceLikeNamespace" },
                 new ClassEntity { Name = "ClassWithNamespaceWithWeirdCase", FullName = "Blyros.Tests.Data.NAmeSPace.ClassWithNamespaceWithWeirdCase" },
            };

            classes.Should().HaveCount(3);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithInterfaceOfType()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithInterface().OfType<IInterface>()
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassImplementingIInterface", FullName = "Blyros.Tests.Data.ClassImplementingIInterface" },
            };

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithAttributeAnyType()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithAttribute().AnyType()
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "TestAttribute", FullName = "Blyros.Tests.Data.TestAttribute" },
                 new ClassEntity { Name = "ClassWithAttribute", FullName = "Blyros.Tests.Data.ClassWithAttribute" },
                 new ClassEntity { Name = "ClassWithAttributes", FullName = "Blyros.Tests.Data.ClassWithAttributes" },
            };

            classes.Should().HaveCount(3);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithAttributeOfType()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithAttribute().OfType<TestAttribute>()
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassWithAttribute", FullName = "Blyros.Tests.Data.ClassWithAttribute" },
                 new ClassEntity { Name = "ClassWithAttributes", FullName = "Blyros.Tests.Data.ClassWithAttributes" },
            };

            classes.Should().HaveCount(2);
            classes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ClassVisitor_WithAttributeOfTypeAllOf()
        {
            var builder = BlyrosBuilder
                .GetClasses()
                .WithAttribute().OfType().AllOf(typeof(ObsoleteAttribute), typeof(TestAttribute))
                .Build();

            var classes = builder.Execute(typeof(Struct));

            var expected = new[]
            {
                 new ClassEntity { Name = "ClassWithAttributes", FullName = "Blyros.Tests.Data.ClassWithAttributes" },
            };

            classes.Should().HaveCount(1);
            classes.Should().BeEquivalentTo(expected);
        }
    }
}