using System;

namespace NameInProgress.Builders
{
    internal interface INameChecker
    {
        Func<string, bool> NameChecker { get; set; }
    }
}