using System;
using System.Collections.Generic;

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
            NameInProgressBuilder builder = new NameInProgressBuilder()
                .GetClasses()
                .WithName("Filter")
                .OnlyPublics()
                .Build();
            IEnumerable<object> classes = builder.Execute(typeof(Strinken.Parser.IToken).Assembly.Location);
            foreach (var @class in classes)
            {
                Console.WriteLine(@class.ToString());
            }
        }
    }
}