using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Querying;
using EPiServer.Core;
using EPiServer.Find;

namespace EPiServer.Reference.Commerce.Site.Features.Test
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            for (int i = 0; i < 5; i++)
            {
                var e = new Employee()
                {
                    Age = i,
                    Gender = true,
                    Name = "ABC " + i
                };
                ContentDataQueryHandler.Instance.Create().Search<PageData>().Filter(x=>x.PageName.Match("ABC"));
            }
            return View();
        }
    }
}