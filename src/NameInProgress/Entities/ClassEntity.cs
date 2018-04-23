namespace NameInProgress.Entities
{
    /// <summary>
    /// Represents a class.
    /// </summary>
    public class ClassEntity
    {
        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the full name of the class (namespace and name).
        /// </summary>
        public string FullName { get; internal set; }
    }
}