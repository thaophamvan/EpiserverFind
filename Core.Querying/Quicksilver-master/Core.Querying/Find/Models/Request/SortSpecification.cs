using System.Collections.Generic;

namespace Core.Querying.Find.Models.Request
{
    public class SortSpecification
    {
        internal readonly IList<SortItem> Items;

        public SortSpecification()
        {
            this.Items = new List<SortItem>();
        }

        public void Add(SortItem item)
        {
            this.Items.Add(item);
        }
    }

    public class SortItem 
    {
        public string Field { get; set; }

        public bool Ascending { get; set; }

        public SortItem(bool ascending, string field)
        {
            this.Ascending = ascending;
            this.Field = field;
        }
    }
}