using NameInProgress.Conditions;
using NameInProgress.Entities;
using NameInProgress.Visitors;

namespace NameInProgress.Builders
{
    public interface IClassVisitorBuilder
    {
        IVisitor<ClassEntity> Build();

        IGenericParameterCondition<IClassVisitorBuilder> WithGenericParameter();

        IAccessibilityCondition<IClassVisitorBuilder> WithAccessibility();

        INameCondition<IClassVisitorBuilder> WithName();
    }
}