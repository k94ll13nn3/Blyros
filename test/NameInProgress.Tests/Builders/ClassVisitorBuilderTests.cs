using System.Collections.Immutable;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using NameInProgress.Builders;
using NameInProgress.Enums;
using NameInProgress.Visitors;
using Xunit;

namespace NameInProgress.Tests.Builders
{
    public class ClassVisitorBuilderTests
    {
        [Fact]
        public void WithName_WithGoodParameter_ReturnsTrue()
        {
            var builder = new ClassVisitorBuilder().WithName().EqualTo(nameof(ClassVisitorBuilder)) as ClassVisitorBuilder;

            builder.NameChecker(nameof(ClassVisitorBuilder)).Should().BeTrue();
        }

        [Fact]
        public void WithName_WithBadParameter_ReturnsFalse()
        {
            var builder = new ClassVisitorBuilder().WithName().EqualTo(nameof(ClassVisitorBuilder)) as ClassVisitorBuilder;

            builder.NameChecker(nameof(ClassVisitorBuilderTests)).Should().BeFalse();
        }

        [Fact]
        public void WithAccessibility_WithGoodParameter_ReturnsTrue()
        {
            var builder = new ClassVisitorBuilder().WithAccessibility().EqualTo(MemberAccessibility.Internal) as ClassVisitorBuilder;

            builder.AccessibilityChecker(Accessibility.Internal).Should().BeTrue();
        }

        [Fact]
        public void WithAccessibility_WithBadParameter_ReturnsFalse()
        {
            var builder = new ClassVisitorBuilder().WithAccessibility().EqualTo(MemberAccessibility.Internal) as ClassVisitorBuilder;

            builder.AccessibilityChecker(Accessibility.Public).Should().BeFalse();
        }

        [Fact]
        public void WithGenericParameter_WithGoodParameter_ReturnsTrue()
        {
            var builder = new ClassVisitorBuilder().WithGenericParameter().OfType<ClassVisitorBuilder>() as ClassVisitorBuilder;

            builder.GenericParameterChecker(typeof(ClassVisitorBuilder).GetFakeTypeParameterSymbol()).Should().BeTrue();
        }

        [Fact]
        public void WithGenericParameter_WithBadParameter_ReturnsFalse()
        {
            var builder = new ClassVisitorBuilder().WithGenericParameter().OfType<ClassVisitorBuilder>() as ClassVisitorBuilder;

            builder.GenericParameterChecker(typeof(ClassVisitorBuilderTests).GetFakeTypeParameterSymbol()).Should().BeFalse();
        }

        [Fact]
        public void WithNamespace_WithGoodParameter_ReturnsTrue()
        {
            var builder = new ClassVisitorBuilder().WithNamespace().EqualTo("NameInProgress.Tests.Builders") as ClassVisitorBuilder;

            builder.NamespaceChecker("NameInProgress.Tests.Builders").Should().BeTrue();
        }

        [Fact]
        public void WithNamespace_WithBadParameter_ReturnsFalse()
        {
            var builder = new ClassVisitorBuilder().WithNamespace().EqualTo("NameInProgress.Tests.Builders") as ClassVisitorBuilder;

            builder.NamespaceChecker("NameInProgress.Tests.Visitors").Should().BeFalse();
        }

        [Fact]
        public void WithInterface_WithGoodParameter_ReturnsTrue()
        {
            var builder = new ClassVisitorBuilder().WithInterface().OfType<ClassVisitorBuilder>() as ClassVisitorBuilder;

            builder.InterfaceChecker(typeof(ClassVisitorBuilder).GetFakeTypeSymbols()).Should().BeTrue();
        }

        [Fact]
        public void WithInterface_WithBadParameter_ReturnsFalse()
        {
            var builder = new ClassVisitorBuilder().WithInterface().OfType<ClassVisitorBuilder>() as ClassVisitorBuilder;

            builder.InterfaceChecker(typeof(ClassVisitorBuilderTests).GetFakeTypeSymbols()).Should().BeFalse();
        }

        [Fact]
        public void Build_Call_ReturnsAClassVisitor()
        {
            new ClassVisitorBuilder().Build().Should().BeOfType<ClassVisitor>();
        }
    }
}