using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Blyros.Runner
{
    internal class Program
    {
        private readonly static SymbolDisplayFormat FullSymbolDisplayFormat = new SymbolDisplayFormat(
            typeQualificationStyle:
                SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            genericsOptions:
                SymbolDisplayGenericsOptions.IncludeTypeConstraints
                | SymbolDisplayGenericsOptions.IncludeVariance
                | SymbolDisplayGenericsOptions.IncludeTypeParameters,
            memberOptions:
                SymbolDisplayMemberOptions.IncludeAccessibility
                | SymbolDisplayMemberOptions.IncludeExplicitInterface
                | SymbolDisplayMemberOptions.IncludeModifiers
                | SymbolDisplayMemberOptions.IncludeParameters
                | SymbolDisplayMemberOptions.IncludeType,
            delegateStyle:
                SymbolDisplayDelegateStyle.NameAndSignature,
            extensionMethodStyle:
                SymbolDisplayExtensionMethodStyle.StaticMethod,
            parameterOptions:
                SymbolDisplayParameterOptions.IncludeDefaultValue
                | SymbolDisplayParameterOptions.IncludeExtensionThis
                | SymbolDisplayParameterOptions.IncludeName
                | SymbolDisplayParameterOptions.IncludeParamsRefOut
                | SymbolDisplayParameterOptions.IncludeType,
            propertyStyle:
                SymbolDisplayPropertyStyle.ShowReadWriteDescriptor,
            kindOptions:
                SymbolDisplayKindOptions.IncludeTypeKeyword
                | SymbolDisplayKindOptions.IncludeMemberKeyword
                | SymbolDisplayKindOptions.IncludeNamespaceKeyword,
            miscellaneousOptions:
                SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

        private static void Main(string[] args)
        {
            IEnumerable<ISymbol> symbols = BlyrosSymbolVisitor
                .Create()
                .WithOptions(BlyrosSymbolVisitorOptions.Default)
                .WithNamespaceFilter(n => n.ToString().Contains("Blyros"))
                .Execute(typeof(BlyrosSymbolVisitor));
            foreach (ISymbol symbol in symbols.Take(5))
            {
                Console.WriteLine($"{symbol.ToDisplayString(FullSymbolDisplayFormat)}");
            }
        }
    }
}
