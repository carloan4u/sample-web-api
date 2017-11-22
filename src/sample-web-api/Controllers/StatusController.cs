using System.Net;
using System.Web.Http;
using System.Configuration;

namespace sample_website.Controllers
{
    public class StatusController : ApiController
    {
        public dynamic Get()
        {
            return new
            {
                Status = ConfigurationManager.AppSettings["Environment"],
                SampleConfigItem = ConfigurationManager.AppSettings["SampleConfigItem"],
                SecretConfigItem = ConfigurationManager.AppSettings["SecretConfigItem"]
            };
        }
    }
}
