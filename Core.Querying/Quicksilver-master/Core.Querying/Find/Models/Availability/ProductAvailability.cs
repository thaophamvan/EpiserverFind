namespace ore.Querying.Find.Models.Availability
{
    public class ProductAvailability
    {
        public string Market { get; set; }
        public string CurrencyCode { get; set; }
        public string WarehouseCode { get; set; }
        public bool IsAvailable { get; set; }
    }
}