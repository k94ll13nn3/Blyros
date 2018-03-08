using NameInProgress.Interfaces;

namespace NameInProgress
{
    public static class NameInProgressBuilder
    {
        public static IClassVisitorBuilder GetClasses() => new ClassVisitorBuilder();
    }
}