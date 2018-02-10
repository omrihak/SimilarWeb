using SimilarWebAPI.Manager;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SimilarWebAPI.Controllers
{
    public class FollowersController : ApiController
    {
        [ResponseType(typeof(string))]
        [Route("api/followers/{followingUserName}/{followedUserName}")]
        public HttpResponseMessage Put(HttpRequestMessage request, string followingUserName, string followedUserName)
        {
            var result = FollowersManager.AddNewFollower(followingUserName.ToLower(), followedUserName.ToLower());

            return request.CreateResponse(result.Success ? HttpStatusCode.OK : HttpStatusCode.Conflict, result.Data);
        }

        [ResponseType(typeof(string))]
        [Route("api/followers/{followingUserName}/{followedUserName}")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string followingUserName, string followedUserName)
        {
            var result = FollowersManager.DeleteFollower(followingUserName.ToLower(), followedUserName.ToLower());

            return request.CreateResponse(result.Success ? HttpStatusCode.OK : HttpStatusCode.Conflict, result.Data);
        }
    }
}
