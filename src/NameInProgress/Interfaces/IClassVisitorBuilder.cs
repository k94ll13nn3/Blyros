namespace NameInProgress.Interfaces
{
    public interface IClassVisitorBuilder
    {
        IClassVisitorBuilder WithName(string name);

        IClassVisitorBuilder OnlyGenerics();

        IClassVisitorBuilder OnlyPublics();

        IVisitor Build();
    }
}