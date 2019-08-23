using System.Collections.Generic;

namespace Core.Querying.Find.Interfaces
{
    public interface IIndexService
    {
        void DoReIndexProducts(IEnumerable<string> catalogEntryCodes);
    }
}