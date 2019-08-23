namespace Core.Querying.Shared
{
    public class GlobalConstants
    {
 
        #region Default Values
        public static class DefaultValues
        {
            public const string All = "All";
            public const string Facet = "Facet";
            public const string Yes = "Yes";
            public const string No = "No";
            public const string T = "T";
            public const int DefaultPriceInterval = 100;
        }
        #endregion

        #region Signs
        public static class Signs
        {
            public const string Comma = ",";
            public const string VerticalBar = "|";
            public const string Equality = "=";
            public const string Hyphen = "-";
            public const string Ampersand = "&";
        }
        #endregion

        #region Market
        public static class Market
        {
            public const string Australia = "AU";
            public const string NewZealand = "NZ";
        }
        #endregion

        #region Facet Data Types
        public static class FacetDataTypes
        {
            public const string Number = "number";
            public const string Boolean = "bool";
        }
        #endregion
    }
}
