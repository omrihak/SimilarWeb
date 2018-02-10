using SimilarWebAPI.Manager;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SimilarWebAPI.Controllers
{
    public class FeedController : ApiController
    {
        [ResponseType(typeof(string))]
        [Route("api/feed/{userName}")]
        public HttpResponseMessage Get(HttpRequestMessage request, string userName)
        {
            var result = FeedManager.GetAllMessagesForUser(userName.ToLower());

            return request.CreateResponse(result.Success ? HttpStatusCode.OK : HttpStatusCode.Conflict, result.Data);
        }

        [ResponseType(typeof(string))]
        [Route("api/feed")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var result = FeedManager.GetAllMessages();

            return request.CreateResponse(result.Success ? HttpStatusCode.OK : HttpStatusCode.Conflict, result.Data);
        }
    }
}
