using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Blyros.Runner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IEnumerable<ISymbol> symbols = BlyrosSymbolVisitor
                .Create()
                .WithOptions(BlyrosSymbolVisitorOptions.Default)
                .Execute(typeof(Program));
            foreach (ISymbol symbol in symbols)
            {
                Console.WriteLine($"{symbol.ToDisplayString()}");
            }
        }
    }
}
