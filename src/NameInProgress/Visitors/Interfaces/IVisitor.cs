﻿using System.Collections.Generic;

namespace NameInProgress.Visitors
{
    public interface IVisitor
    {
        IEnumerable<object> Execute(string location);
    }
}