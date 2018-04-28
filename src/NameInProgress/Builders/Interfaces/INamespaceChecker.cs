using System;

namespace NameInProgress.Builders
{
    /// <summary>
    /// Interface for builders that needs namespace checking.
    /// </summary>
    public interface INamespaceChecker
    {
        /// <summary>
        /// Gets or sets the function used to check namespace.
        /// </summary>
        Func<string, bool> NamespaceChecker { get; set; }
    }
}