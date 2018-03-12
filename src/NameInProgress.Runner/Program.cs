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
                .WithName().Like("visi", true)
                .WithAccessibility().OneOf(MemberAccessibility.Internal, MemberAccessibility.Public)
                .WithGenericParameter().AnyType()
                .WithGenericParameter().OfType<object>()
                .WithGenericParameter().OfType().AllOf(typeof(object))
                .WithGenericParameter().WithConstraint().EqualTo(true).AnyType()
                .WithGenericParameter().WithConstraint().OneOf().AnyType()
                .WithGenericParameter().WithConstraint().AllOf().OfType<object>()
                .Build();
            var classes = builder.Execute(typeof(NameInProgressBuilder));
            foreach (var @class in classes)
            {
                Console.WriteLine(@class.ToString());
            }
        }
    }
}