using Blyros.Core;

namespace Blyros
{
    public class BlyrosSymbolVisitorOptions
    {
        public static BlyrosSymbolVisitorOptions Default { get; } = new BlyrosSymbolVisitorOptions
        {
            GetClasses = true,
            GetMethods = true,
            GetParameters = true,
            GetProperties = true,
            GetFields = true,
            GetInterfaces = true,
            GetStructs = true,
            GetEnums = true,
            GetNamespaces = true,
            GetTypeParameters = true,
        };

        public bool GetClasses { get; set; }

        public bool GetMethods { get; set; }

        public bool GetParameters { get; set; }

        public bool GetProperties { get; set; }

        public bool GetFields { get; set; }

        public bool GetInterfaces { get; set; }

        public bool GetStructs { get; set; }

        public bool GetEnums { get; set; }

        public bool GetNamespaces { get; set; }

        public bool GetTypeParameters { get; set; }

        internal VisitorDepth GetVisitorDepth()
        {
            VisitorDepth depth = VisitorDepth.None;
            depth = GetNamespaces ? VisitorDepth.Namespace : depth;
            depth = GetInterfaces || GetClasses || GetStructs || GetEnums ? VisitorDepth.NamedType : depth;
            depth = GetMethods || GetProperties || GetFields ? VisitorDepth.Members : depth;
            depth = GetParameters ? VisitorDepth.Parameters : depth;

            return depth;
        }
    }
}
