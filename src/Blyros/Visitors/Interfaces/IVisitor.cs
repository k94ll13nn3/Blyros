using System;
using System.Collections.Generic;

namespace Blyros.Visitors
{
    /// <summary>
    /// Interface for visitors.
    /// </summary>
    /// <typeparam name="T">The type of entity to visit.</typeparam>
    public interface IVisitor<T>
    {
        /// <summary>
        /// Executes the visitor on the specified assembly.
        /// </summary>
        /// <param name="location">The location of the assembly.</param>
        /// <returns>A list of matching entities.</returns>
        IEnumerable<T> Execute(string location);

        /// <summary>
        /// Executes the visitor on the assembly of the specified type.
        /// </summary>
        /// <param name="type">TA type included in the assembly to visit.</param>
        /// <returns>A list of matching entities.</returns>
        IEnumerable<T> Execute(Type type);
    }
}