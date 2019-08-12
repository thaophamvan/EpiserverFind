using System.Collections.Generic;

namespace Core.Querying.AutoComplete
{
    public interface IAutoCompleteHandlerController
    {
        IEnumerable<AutoCompleteItem> GetItems(string value, int maxItemCount);
    }
}