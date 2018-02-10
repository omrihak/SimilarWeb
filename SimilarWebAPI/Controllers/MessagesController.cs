using SimilarWebAPI.Manager;
using SimilarWebAPI.Models;
using System;
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
        public HttpResponseMessage Post(HttpRequestMessage request, string userName, [FromBody]string message)
        {
            if(message == null)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, "The message was not accepted properly");
            }
            try
            {
                ResultModel<String> result = MessagesManager.AddNewMessage(userName.ToLower(), message);

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
