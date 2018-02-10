using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SimilarWebAPI.Manager.Controllers
{
    public class UsersController : ApiController
    {
        [ResponseType(typeof(string))]
        [Route("api/users/{username}")]
        public HttpResponseMessage Put(HttpRequestMessage request, string username)
        {
            var result = UsersManager.AddNewUser(username.ToLower());

            return request.CreateResponse(result.Success ? HttpStatusCode.OK : HttpStatusCode.Conflict, result.Data);
        }
    }
}
