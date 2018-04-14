namespace NameInProgress.Enums
{
    /// <summary>
    /// Generic parameter constraint.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters"/>
    public enum GenericConstraint
    {
        /// <summary>
        /// Reference type constraint.
        /// </summary>
        Class,

        /// <summary>
        /// Value type constraint.
        /// </summary>
        Struct,

        /// <summary>
        /// Public parameterless constructor constraint.
        /// </summary>
        New
    }
}