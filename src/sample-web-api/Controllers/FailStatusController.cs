using System.Net;
using System.Web.Http;

namespace sample_website.Controllers
{
    public class FailStatusController : ApiController
    {
        public dynamic Get()
        {
            return new
            {
                Status = HttpStatusCode.InternalServerError
            };
        }
    }
}