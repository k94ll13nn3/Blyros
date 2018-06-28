﻿using System;
using System.Collections.Generic;
using Blyros.Core;
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
                .WithOptions(new BlyrosSymbolVisitorOptions
                {
                    GetParameters = true,
                    GetMethods = true,
                })
                .WithMethodFilter(m => m.IsExtensionMethod)
                .WithParameterFilter(p => p.Name == "symbolVisitor")
                .Visit(typeof(BlyrosSymbolVisitor));
            foreach (ISymbol symbol in symbols)
            {
                Console.WriteLine($"{symbol.ToDisplayString(FullSymbolDisplayFormat)}");
            }
        }
    }
}
