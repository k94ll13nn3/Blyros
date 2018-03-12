using NameInProgress.Visitors;

namespace NameInProgress.Builders
{
    public interface IClassVisitorBuilder
    {
        IVisitor Build();

        IGenericParameterConditionBuilder<IClassVisitorBuilder> WithGenericParameter();

        IAccessibilityConditionBuilder<IClassVisitorBuilder> WithAccessibility();

        INameConditionBuilder<IClassVisitorBuilder> WithName();
    }
}