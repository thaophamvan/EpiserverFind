using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find;

namespace Core.Querying
{
    public class Test
    {
        public void TestMethod()
        {
            ContentDataQueryHandler.Instance.Create().Search<Employee>().Filter(x => x.Gender.Match(true));
        }
    }
}
