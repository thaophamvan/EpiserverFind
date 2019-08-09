using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;

namespace Core.Querying.AutoComplete
{
    public abstract class ContentReferenceCollectionAutoCompleteControllerBase<TPageData> : IAutoCompleteController<PageReferenceCollection>, IAutoCompleteController<PageReference>
        where TPageData : PageData
    {
        //protected IPageRepository PageRepository { get; private set; }

        //protected PageReferenceCollectionAutoCompleteControllerBase(IPageRepository pageRepository)
        //{
        //    this.PageRepository = pageRepository;
        //}

        //public abstract IEnumerable<AutoCompleteItem> GetItems(string value, int maxItemCount);

        //public abstract int MaxItemNumber { get; }

        //public virtual PageReferenceCollection ValidateAndConvert(IEnumerable<AutoCompleteItem> selectedItems, PropertyData propertyData, IControlErrorLog validatorControl)
        //{
        //    var collection = new PageReferenceCollection();

        //    if (selectedItems.Count() > MaxItemNumber)
        //    {
        //        validatorControl.AddErrorValidator("errortoomanyentries");
        //        return new PageReferenceCollection { PageReference.EmptyReference };
        //    }

        //    foreach (var item in selectedItems)
        //    {
        //        PageReference link;
        //        if (string.IsNullOrEmpty(item.Id))
        //        {
        //            link = FindByText(item.Text);
        //            if (PageReference.IsNullOrEmpty(link))
        //            {
        //                link = ResolveNotFoundItem(item.Text, propertyData, validatorControl);
        //                if (PageReference.IsNullOrEmpty(link))
        //                {
        //                    return new PageReferenceCollection { PageReference.EmptyReference };
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // TODO: validate item.Id
        //            link = PageReference.Parse(item.Id);
        //        }
        //        if (!PageReference.IsNullOrEmpty(link))
        //        {
        //            collection.Add(link);
        //        }
        //    }

        //    return collection;
        //}

        //PageReference IAutoCompleteController<PageReference>.ValidateAndConvert(IEnumerable<AutoCompleteItem> selectedItems, PropertyData propertyData, IControlErrorLog validatorControl)
        //{
        //    return ValidateAndConvert(selectedItems, propertyData, validatorControl).FirstOrDefault();
        //}

        //public virtual IEnumerable<AutoCompleteItem> GetInitialItems(PropertyData propertyData)
        //{
        //    PageReferenceCollection links;
        //    if (propertyData.Value is PageReferenceCollection)
        //    {
        //        links = propertyData.Value as PageReferenceCollection;
        //    }
        //    else if (propertyData.Value is PageReference)
        //    {
        //        links = new PageReferenceCollection() { (PageReference)propertyData.Value };
        //    }
        //    else
        //    {
        //        throw new InvalidPropertyValueException("Property data should be PageReference or PageReferenceCollection", propertyData.Value.GetType().ToString());
        //    }
        //    var pages = PageRepository.LoadPages<TPageData>(links);
        //    return pages.Select(GetItemFromPage);
        //}

        //protected virtual AutoCompleteItem GetItemFromPage(TPageData page)
        //{
        //    return new AutoCompleteItem(page.PageLink.ToStringIgnoreWorkId(), page.PageName.HtmlAttributeEncode());
        //}

        //protected abstract PageReference FindByText(string text);

        //protected virtual PageReference ResolveNotFoundItem(string text, PropertyData propertyData, IControlErrorLog validatorControl)
        //{
        //    validatorControl.AddErrorValidator("errorpagenotfoundbytext", text);
        //    return null;
        //}
        public IEnumerable<AutoCompleteItem> GetItems(string value, int maxItemCount)
        {
            throw new System.NotImplementedException();
        }
    }
}