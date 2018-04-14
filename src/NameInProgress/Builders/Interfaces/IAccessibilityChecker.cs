using System;
using Microsoft.CodeAnalysis;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Interface for builders that needs accessibility checking.
    /// </summary>
    internal interface IAccessibilityChecker
    {
        /// <summary>
        /// Gets or sets the function used to check the accessibility.
        /// </summary>
        Func<Accessibility, bool> AccessibilityChecker { get; set; }
    }
}