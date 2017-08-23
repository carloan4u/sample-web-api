using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace sample_website.Controllers
{
    public class StatusController : ApiController
    {
        public dynamic Get()
        {
            return new
            {
                Status = ConfigurationManager.AppSettings["Environment"],
                SampleConfigItem = ConfigurationManager.AppSettings["SampleConfigItem"]
            };
        }
    }
}
