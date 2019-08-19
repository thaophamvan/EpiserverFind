using Core.Querying;
using EPiServer.Find;
using EPiServer.Find.Framework;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace EPiServer.Reference.Commerce.Site.Features.Test
{
    public class TestController : Controller
    {
        private readonly ITicketProvider _ticketProvider;
        private readonly IClient _client;

        public TestController(ITicketProvider ticketProvider)
        {
            _ticketProvider = ticketProvider;
            _client = SearchClient.Instance;
        }

        // GET: Test
        //public async Task<ActionResult> Index(int q = 100)
        //{
        //    var searchTasks = Enumerable.Range(1, q).Select(i => SearchAsync(_ticketProvider, i));

        //    var searchResults = await Task.WhenAll(searchTasks);

        //    return Content(JsonConvert.SerializeObject(searchResults));
        //}

        //private async Task<SearchResults<Employee>> SearchAsync(ITicketProvider ticketProvider, int i)
        //{
        //    //return await _client.Search<Employee>().Filter(x => x.Name.Match($"ABC {i}")).GetResultAsync(ticketProvider);
            
        //}
    }
}