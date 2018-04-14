using System;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Interface for builders that needs name checking.
    /// </summary>
    internal interface INameChecker
    {
        /// <summary>
        /// Gets or sets the function used to check name.
        /// </summary>
        Func<string, bool> NameChecker { get; set; }
    }
}