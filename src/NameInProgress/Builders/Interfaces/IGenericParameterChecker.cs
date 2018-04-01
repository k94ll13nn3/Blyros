using System;
using Microsoft.CodeAnalysis;

namespace NameInProgress.Builders
{
    internal interface IGenericParameterChecker
    {
        Func<ITypeParameterSymbol, bool> GenericParameterChecker { get; set; }
    }
}