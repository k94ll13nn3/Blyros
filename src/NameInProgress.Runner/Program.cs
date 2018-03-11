using System;
using System.Collections.Generic;
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
                .WithAccessibility().OneOf(new List<MemberAccessibility> { MemberAccessibility.Internal })
                .Build();
            var classes = builder.Execute(typeof(NameInProgressBuilder));
            foreach (var @class in classes)
            {
                Console.WriteLine(@class.ToString());
            }
        }
    }
}