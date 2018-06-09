using System;
using Microsoft.CodeAnalysis;

namespace Blyros.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var symbolDisplayFormat = new SymbolDisplayFormat(
              typeQualificationStyle:
                  SymbolDisplayTypeQualificationStyle.NameAndContainingTypes,
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

            var symbols = new BlyrosSymbolVisitor().Execute(typeof(BlyrosSymbolVisitor));
            foreach (var symbol in symbols)
            {
                Console.WriteLine($"{symbol.ToDisplayString(symbolDisplayFormat)}");
            }
        }
    }
}
