using SimilarWebAPI.Manager;
using SimilarWebAPI.Models;
using System;
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
            try
            {
                ResultModel<String> result = FollowersManager.AddNewFollower(followingUserName.ToLower(), followedUserName.ToLower());

                return request.CreateResponse(result.Success ? HttpStatusCode.OK : HttpStatusCode.Conflict, result.Data);
            }
            catch (Exception ex)
            {
                LoggerManager.Log(ex.Message);
                return request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ResponseType(typeof(string))]
        [Route("api/followers/{followingUserName}/{followedUserName}")]
        public HttpResponseMessage Delete(HttpRequestMessage request, string followingUserName, string followedUserName)
        {
            try
            {
                ResultModel<String> result = FollowersManager.DeleteFollower(followingUserName.ToLower(), followedUserName.ToLower());
                return request.CreateResponse(result.Success ? HttpStatusCode.OK : HttpStatusCode.Conflict, result.Data);
            }
            catch (Exception ex)
            {
                LoggerManager.Log(ex.Message);
                return request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
