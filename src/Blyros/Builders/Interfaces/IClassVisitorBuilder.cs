using Blyros.Conditions;
using Blyros.Entities;
using Blyros.Visitors;

namespace Blyros.Builders
{
    /// <summary>
    /// Interface for exposing the class visitor builder.
    /// </summary>
    public interface IClassVisitorBuilder
    {
        /// <summary>
        /// Builds the visitor and returns it.
        /// </summary>
        /// <returns>The visitor.</returns>
        IVisitor<ClassEntity> Build();

        /// <summary>
        /// Returns a builder for building a condition on generic parameter.
        /// </summary>
        /// <returns>The builder.</returns>
        IGenericParameterCondition<IClassVisitorBuilder> WithGenericParameter();

        /// <summary>
        /// Returns a builder for building a condition on accessibility.
        /// </summary>
        /// <returns>The builder.</returns>
        IAccessibilityCondition<IClassVisitorBuilder> WithAccessibility();

        /// <summary>
        /// Returns a builder for building a condition on name.
        /// </summary>
        /// <returns>The builder.</returns>
        IStringCondition<IClassVisitorBuilder> WithName();

        /// <summary>
        /// Returns a builder for building a condition on namespace.
        /// </summary>
        /// <returns>The builder.</returns>
        IStringCondition<IClassVisitorBuilder> WithNamespace();

        /// <summary>
        /// Returns a builder for building a condition on interfaces.
        /// </summary>
        /// <returns>The builder.</returns>
        ITypeCondition<IClassVisitorBuilder> WithInterface();
    }
}