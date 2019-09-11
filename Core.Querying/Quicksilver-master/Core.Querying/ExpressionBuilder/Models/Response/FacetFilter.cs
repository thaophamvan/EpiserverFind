namespace Core.Querying.ExpressionBuilder.Models.Response
{
    public class FacetFilter
    {
        public string GroupName { get; set; }

        public string GroupKey { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public int Count { get; set; }

        public bool Filtered { get; set; }

        public int SortOrder { get; set; }
    }
}