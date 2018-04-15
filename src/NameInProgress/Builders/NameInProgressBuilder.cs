namespace NameInProgress.Builders
{
    /// <summary>
    /// Entry point for creating all the builders.
    /// </summary>
    public static class NameInProgressBuilder
    {
        /// <summary>
        /// Returns a builder to create a vitisor on classes.
        /// </summary>
        /// <returns>The builder.</returns>
        public static IClassVisitorBuilder GetClasses() => new ClassVisitorBuilder();
    }
}