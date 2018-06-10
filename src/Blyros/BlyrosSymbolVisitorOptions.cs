namespace Blyros
{
    public class BlyrosSymbolVisitorOptions
    {
        public static BlyrosSymbolVisitorOptions Default { get; } = new BlyrosSymbolVisitorOptions
        {
            GetClasses = true,
        };

        public bool GetClasses { get; set; }
    }
}
