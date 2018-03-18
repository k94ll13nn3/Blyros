using System;
using Microsoft.CodeAnalysis;

namespace NameInProgress.Builders
{
    internal interface IGenericParameterCondition
    {
        Func<ITypeParameterSymbol, bool> GenericParameterChecker { get; set; }
    }
}