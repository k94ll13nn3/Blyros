using NameInProgress.Entities;
using NameInProgress.Visitors;

namespace NameInProgress.Builders
{
    public interface IClassVisitorBuilder
    {
        IVisitor<ClassEntity> Build();

        IGenericParameterConditionBuilder<IClassVisitorBuilder> WithGenericParameter();

        IAccessibilityConditionBuilder<IClassVisitorBuilder> WithAccessibility();

        INameConditionBuilder<IClassVisitorBuilder> WithName();
    }
}