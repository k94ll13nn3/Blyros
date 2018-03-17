using System;
using NameInProgress.Builders;
using NameInProgress.Enums;

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
                //.WithName().Like("visi", true)
                .WithAccessibility().OneOf(MemberAccessibility.Internal, MemberAccessibility.Public)

                // -------------- to move to unit tests
                .WithGenericParameter().AnyType()
                .WithGenericParameter().OfType<ITruc2>()
                .WithGenericParameter().OfType().OneOf(typeof(ITruc2), typeof(IDisposable), typeof(ITruc<DateTime, bool>))
                .WithGenericParameter().OfType().AllOf(typeof(ITruc2), typeof(IDisposable))
                .WithGenericParameter().WithConstraint().EqualTo(GenericConstraint.Class).AnyType()
                .WithGenericParameter().WithConstraint().OneOf(GenericConstraint.Class, GenericConstraint.Struct).AnyType()
                .WithGenericParameter().WithConstraint().AllOf(GenericConstraint.Class, GenericConstraint.New).AnyType()
                .WithGenericParameter().WithConstraint().EqualTo(GenericConstraint.New).AnyType()
                .WithGenericParameter().WithConstraint().EqualTo(GenericConstraint.New).OfType<IDisposable>()
                // -------------- to move to unit tests

                .Build();
            var classes = builder.Execute(typeof(Program));
            foreach (var classEntity in classes)
            {
                Console.WriteLine(classEntity.Name);
            }
        }
    }

    public interface ITruc2
    {
    }

    public interface ITruc<T, V>
    {
    }

    public class Truc<U> where U : ITruc<DateTime, bool>
    {
    }

    public class Truc2<V> where V : class, ITruc2, new()
    {
    }

    public class Truc3<V> where V : ITruc2, IDisposable, new()
    {
    }

    public class Truc4<V> where V : struct
    {
    }
}