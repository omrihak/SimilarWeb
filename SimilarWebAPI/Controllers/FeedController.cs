using SimilarWebAPI.Manager;
using SimilarWebAPI.Models;
using System;
using System.Collections.Generic;
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
            try
            {
                List<Message> messages = FeedManager.GetAllMessagesForUser(userName.ToLower());
                return request.CreateResponse(HttpStatusCode.OK, messages);
            }
            catch (Exception ex)
            {
                LoggerManager.Log(ex.Message);
                return request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ResponseType(typeof(string))]
        [Route("api/feed")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            try
            {
                List<Message> messages = FeedManager.GetAllMessages();
                return request.CreateResponse(HttpStatusCode.OK, messages);
            }
            catch (Exception ex)
            {
                LoggerManager.Log(ex.Message);
                return request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
