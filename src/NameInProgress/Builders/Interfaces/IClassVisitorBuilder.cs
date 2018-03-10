﻿using NameInProgress.Conditions;
using NameInProgress.Visitors;

namespace NameInProgress.Builders
{
    public interface IClassVisitorBuilder : 
        IBuilder, 
        INameCondition<IClassVisitorBuilder>, 
        IAccessibilityCondition<IClassVisitorBuilder>
    {
        IVisitor Build();
    }
}