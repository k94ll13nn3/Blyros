using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Blyros.Extensions
{
    public static class SymbolVisitorExtensions
    {
        /// <summary>
        /// Visits the assembly of the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type of the returned data.</typeparam>
        /// <param name="symbolVisitor">The symbol visitor to use.</param>
        /// <param name="type">A <see cref="Type"/> contained in the assembly to visit.</param>
        /// <returns>The data returned by the visitor.</returns>
        public static T Visit<T>(this SymbolVisitor<T> symbolVisitor, Type type) => Visit(symbolVisitor, type.Assembly.Location);

        /// <summary>
        /// Visits an assembly.
        /// </summary>
        /// <typeparam name="T">The type of the returned data.</typeparam>
        /// <param name="symbolVisitor">The symbol visitor to use.</param>
        /// <param name="path">The path of the assembly to visit.</param>
        /// <returns>The data returned by the visitor.</returns>
        public static T Visit<T>(this SymbolVisitor<T> symbolVisitor, string path)
        {
            MetadataReference testedAssembly = MetadataReference.CreateFromFile(path);

            // TODO: test other platforms (like Mono) to see what is needed.
            IList<MetadataReference> platformAssemblies;
            object trustedPlatformAssemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
            if (trustedPlatformAssemblies != null)
            {
                // .NET Core App need this for resolving types.
                // TODO: see if all assemblies are needed.
                // https://github.com/dotnet/roslyn/wiki/Runtime-code-generation-using-Roslyn-compilations-in-.NET-Core-App
                platformAssemblies = trustedPlatformAssemblies
                    .ToString()
                    .Split(Path.PathSeparator)
                    //.Where(t => t.Contains("System.Runtime.dll") || t.Contains("System.Private.CoreLib.dll") || t.Contains("netstandard.dll"))
                    .Select(x => (MetadataReference)MetadataReference.CreateFromFile(x))
                    .ToList();
            }
            else
            {
                // .NET Framework
                MetadataReference mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                platformAssemblies = new List<MetadataReference> { mscorlib };
            }

            platformAssemblies.Add(testedAssembly);

            Compilation compilation = CSharpCompilation
                .Create(nameof(BlyrosSymbolVisitor))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithMetadataImportOptions(MetadataImportOptions.All))
                .WithReferences(platformAssemblies);

            return symbolVisitor.Visit(compilation.GetAssemblyOrModuleSymbol(testedAssembly));
        }
    }
}
