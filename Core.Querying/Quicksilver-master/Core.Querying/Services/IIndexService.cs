using System.Collections.Generic;

namespace Core.Querying.Services
{
    public interface IIndexService
    {
        void DoReIndexProducts(IEnumerable<string> catalogEntryCodes);
    }
}