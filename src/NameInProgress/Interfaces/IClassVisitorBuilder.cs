namespace NameInProgress.Interfaces
{
    public interface IClassVisitorBuilder : IBuilder, INameCondition<IClassVisitorBuilder>, IAccessibilityCondition<IClassVisitorBuilder>
    {
        IVisitor Build();
    }
}