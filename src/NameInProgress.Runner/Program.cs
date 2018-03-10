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
                .WithName().Like("")
                .WithAccessibility().EqualTo(MemberAccessibility.Public)
                .Build();
            var classes = builder.Execute(typeof(NameInProgressBuilder).Assembly.Location);
            foreach (var @class in classes)
            {
                Console.WriteLine(@class.ToString());
            }
        }
    }
}