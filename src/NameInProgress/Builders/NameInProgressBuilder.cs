namespace NameInProgress.Builders
{
    public static class NameInProgressBuilder
    {
        public static IClassVisitorBuilder GetClasses() => new ClassVisitorBuilder();
    }
}