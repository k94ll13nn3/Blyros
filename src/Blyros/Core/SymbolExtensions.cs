using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace Blyros.Core
{
    internal static class SymbolExtensions
    {
        public static bool IsCompilerGenerated(this ISymbol symbol) =>
            symbol.GetAttributes().Any(a => a.AttributeClass.Name == nameof(CompilerGeneratedAttribute));

        public static bool IsSpecial(this ISymbol symbol)
        {
            // "<global namespace>" and "<Module>" and others (if there is any...).
            string symbolAsString = symbol.ToString();
            return symbolAsString.StartsWith("<") && symbolAsString.EndsWith(">");
        }
    }
}
