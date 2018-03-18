using System;

namespace NameInProgress.Builders
{
    internal interface INameCondition
    {
        Func<string, bool> NameChecker { get; set; }
    }
}