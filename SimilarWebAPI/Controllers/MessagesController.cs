using SimilarWebAPI.Manager;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SimilarWebAPI.Controllers
{
    public class MessagesController : ApiController
    {
        [ResponseType(typeof(string))]
        [Route("api/messages/{userName}")]
        public HttpResponseMessage Post(HttpRequestMessage request, string userName)
        {
            string message = request.Content.ReadAsStringAsync().Result;
            var result = MessagesManager.AddNewMessage(userName.ToLower(), message);

            return request.CreateResponse(result.Success ? HttpStatusCode.OK : HttpStatusCode.Conflict, result.Data);
        }
    }
}
