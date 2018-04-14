using System;
using Microsoft.CodeAnalysis;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Interface for builders that needs generic parameters checking.
    /// </summary>
    internal interface IGenericParameterChecker
    {
        /// <summary>
        /// Gets or sets the function used to check generic parameters.
        /// </summary>
        Func<ITypeParameterSymbol, bool> GenericParameterChecker { get; set; }
    }
}