using System;
using System.Collections.Generic;
using System.Globalization;
using EPiServer.Find;

namespace Core.Querying.Find.Models.Request
{
    public class FilterSpecification
    {
        public IList<FilterItem> Items { get; set; }

        public CultureInfo CultureInfo { get; set; }

        public Language Language
        {
            get
            {
                if (this.CultureInfo != null)
                {
                    return new Language(this.CultureInfo.EnglishName, this.CultureInfo.EnglishName.ToLower(), this.CultureInfo.Name.ToLower(), "porter", null);
                }
                return null;
            }
        }

        public FilterSpecification()
        {
            Items = new List<FilterItem>();
        }

        public void Add(FilterItem item)
        {
            this.Items.Add(item);
        }
    }

    public class FilterItem 
    {
        public string Field { get; set; }
    }

    public class MatchFilterItem : FilterItem
    {
        public IList<object> Value { get; set; }

        public MatchFilterItem(string field, IList<object> value)
        {
            this.Value = value;
            this.Field = field;
        }
    }

    public class InRangeFilterItem: FilterItem
    {
        public IList<RangeItem> Values { get; set; }
        public InRangeFilterItem(string field, IList<RangeItem> values)
        {
            Field = field;
            Values = values;
        }
    }

    public class RangeItem
    {
        public double From { get; set; }
        public double To { get; set; }
    }


    public class NotMatchFilterItem : MatchFilterItem
    {
        public NotMatchFilterItem(string field, IList<object> value) : base(field, value)
        {
            throw new NotImplementedException("NotMatchFilterItem is not implemented!");
        }
    }
}