namespace NameInProgress.Interfaces
{
    public interface IClassVisitorBuilder : IVisitor, INameCondition<IClassVisitorBuilder>
    {
        IVisitor Build();
    }
}