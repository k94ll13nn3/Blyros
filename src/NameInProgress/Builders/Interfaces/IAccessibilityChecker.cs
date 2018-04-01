using System;
using Microsoft.CodeAnalysis;

namespace NameInProgress.Builders
{
    internal interface IAccessibilityChecker
    {
        Func<Accessibility, bool> AccessibilityChecker { get; set; }
    }
}