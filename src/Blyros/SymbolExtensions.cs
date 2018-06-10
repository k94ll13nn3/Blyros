using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace Blyros
{
    internal static class SymbolExtensions
    {
        public static bool IsCompilerGeneratedSymbol(this ISymbol symbol) => 
            symbol.GetAttributes().Any(a => a.AttributeClass.Name == nameof(CompilerGeneratedAttribute)); 
    }
}
