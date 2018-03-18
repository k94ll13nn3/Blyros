using System;
using Microsoft.CodeAnalysis;

namespace NameInProgress.Builders
{
    internal interface IAccessibilityCondition
    {
        Func<Accessibility, bool> AccessibilityChecker { get; set; }
    }
}