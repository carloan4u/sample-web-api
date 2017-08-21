using System.Net;
using System.Web.Http;

namespace sample_website.Controllers
{
    public class StatusController : ApiController
    {
        public dynamic Get()
        {
            return new
            {
                Status = "OK"
            };
        }
    }

    public class FailStatusController : ApiController
    {
        public dynamic Get()
        {
            return StatusCode(HttpStatusCode.InternalServerError);
        }
    }
}
