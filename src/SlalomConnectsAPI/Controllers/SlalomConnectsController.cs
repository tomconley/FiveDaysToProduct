using SlalomConnectsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace SlalomConnectsAPI.Controllers
{
    public class SlalomConnectsController : ApiController
    {
        // Uncomment following lines for service principal authentication
        //private static string trustedCallerClientId = ConfigurationManager.AppSettings["todo:TrustedCallerClientId"];
        //private static string trustedCallerServicePrincipalId = ConfigurationManager.AppSettings["todo:TrustedCallerServicePrincipalId"];

        private static List<EventRequest> _existingEventRequests;
        private static GroupMatchingController _groupMatchingController;

        static SlalomConnectsController()
        {
            _existingEventRequests = new List<EventRequest>();
            _groupMatchingController = new GroupMatchingController();
            //mockData.Add(0, new ToDoItem { ID = 0, Owner = "*", Description = "feed the dog" });
            //mockData.Add(1, new ToDoItem { ID = 1, Owner = "*", Description = "take the dog on a walk" });
        }

        private static void CheckCallerId()
        {
            // Uncomment following lines for service principal authentication
            //string currentCallerClientId = ClaimsPrincipal.Current.FindFirst("appid").Value;
            //string currentCallerServicePrincipalId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //if (currentCallerClientId != trustedCallerClientId || currentCallerServicePrincipalId != trustedCallerServicePrincipalId)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized, ReasonPhrase = "The appID or service principal ID is not the expected value." });
            //}
        }

        [HttpGet]
        [Route("slalom-connects-api/get-event-requests")]
        public HttpResponseMessage Get()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            var responseBody = _existingEventRequests.Aggregate("", (current, eventRequest) => current + eventRequest.ToString() + Environment.NewLine);
            response.Content = new StringContent(responseBody, Encoding.UTF8);

            return response;
        }

        [HttpPost]
        [Route("slalom-connects-api/post-event-request")]
        public void Post(string email, EventType eventType, DateTime startTime, DateTime endTime, int? minimumGroupSize, int? maximumGroupSize)
        {
            CheckCallerId();

            try
            {
                var eventRequest = new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    Email = email,
                    EventType = eventType,
                    StartTime = startTime,
                    EndTime = endTime
                };

                var foundGroup = _groupMatchingController.MatchGroupFromEventRequests(eventRequest, _existingEventRequests);

                if (foundGroup == null)
                {
                    _existingEventRequests.Add(eventRequest);
                }
            }
            catch (Exception ex)
            {
            }

            //todo.ID = mockData.Count > 0 ? mockData.Keys.Max() + 1 : 1;
            //mockData.Add(todo.ID, todo);
        }
    }
}