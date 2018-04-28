using System;
using NameInProgress.Builders;
using NameInProgress.Enums;
using NameInProgress.Tests.Data;

namespace NameInProgress.Runner
{
    /// <summary>
    /// Temporary project, for testing.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Program started...");
            var builder = NameInProgressBuilder
                .GetClasses()
                .WithName().Like("Truc", true)
                //.WithName().Like("visi", true)
                //.WithAccessibility().OneOf(MemberAccessibility.Internal, MemberAccessibility.Public)

                // -------------- to move to unit tests
                //.WithGenericParameter().AnyType()
                //.WithGenericParameter().OfType<ITruc2>()
                //.WithGenericParameter().OfType().OneOf(typeof(ITruc2), typeof(IDisposable), typeof(ITruc<DateTime, bool>))
                //.WithGenericParameter().OfType().AllOf(typeof(ITruc2), typeof(IDisposable))
                //.WithGenericParameter().WithConstraint().EqualTo(GenericConstraint.Class).AnyType()
                //.WithGenericParameter().WithConstraint().OneOf(GenericConstraint.Class, GenericConstraint.Struct).AnyType()
                //.WithGenericParameter().WithConstraint().AllOf(GenericConstraint.Class, GenericConstraint.New).AnyType()
                //.WithGenericParameter().WithConstraint().EqualTo(GenericConstraint.New).AnyType()
                //.WithGenericParameter().WithConstraint().EqualTo(GenericConstraint.New).OfType<IDisposable>()
                // -------------- to move to unit tests

                .Build();
            var classes = builder.Execute(typeof(IInterface));
            foreach (var classEntity in classes)
            {
                Console.WriteLine(classEntity.Name);
            }
        }
    }
}