using System;
using System.Collections.Generic;

namespace NameInProgress.Visitors
{
    public interface IVisitor<T>
    {
        IEnumerable<T> Execute(string location);

        IEnumerable<T> Execute(Type type);
    }
}