namespace Blyros
{
    public class BlyrosSymbolVisitorOptions
    {
        public static BlyrosSymbolVisitorOptions Default { get; } = new BlyrosSymbolVisitorOptions
        {
            GetClasses = true,
            GetMethods = true,
        };

        public bool GetClasses { get; set; } = true;

        public bool GetMethods { get; set; } = true;
    }
}
