using System.Collections.Generic;
using EPiServer.Core;

namespace Core.Querying.AutoComplete
{
    public interface IAutoCompleteController<TPropertyValue> : IAutoCompleteHandlerController
    {
        //int MaxItemNumber { get; }

        //TPropertyValue ValidateAndConvert(IEnumerable<AutoCompleteItem> selectedItems, PropertyData propertyData, IControlErrorLog validatorControl);

        //IEnumerable<AutoCompleteItem> GetInitialItems(PropertyData propertyData);
    }
}