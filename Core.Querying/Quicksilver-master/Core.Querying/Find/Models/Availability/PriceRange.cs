using System.Collections.Generic;

namespace Core.Querying.Find.Models.Availability
{
    public class PriceRange
    {
        public string MarketId { get; set; }
        public string CurrencyCode { get; set; }
        public double Price { get; set; }
    }

    public class PriceRangeComparer : IEqualityComparer<PriceRange>
    {
        public bool Equals(PriceRange x, PriceRange y)
        {
            return x.MarketId == y.MarketId && x.CurrencyCode == y.CurrencyCode && x.Price == y.Price;
        }

        public int GetHashCode(PriceRange obj)
        {
            return obj.MarketId.GetHashCode() + obj.Price.GetHashCode() + obj.CurrencyCode.GetHashCode();
        }
    }
}