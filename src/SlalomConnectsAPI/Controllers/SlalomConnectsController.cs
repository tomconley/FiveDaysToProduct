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
            var responseBody = _existingEventRequests.Aggregate("", (current, request) => current + request.ToString() + Environment.NewLine);
            response.Content = new StringContent(responseBody, Encoding.UTF8);

            return response;
        }

        [HttpPost]
        [Route("slalom-connects-api/populate-test-users")]
        public HttpResponseMessage PostTestData()
        {
            _existingEventRequests.Add(new EventRequest()
            {
                
            });
            return null;
        }

        [HttpPost]
        [Route("slalom-connects-api/post-event-request")]
        public HttpResponseMessage Post(string email, EventType eventType, DateTime startTime, DateTime endTime, int? maximumGroupSize)
        {
            CheckCallerId();

            try
            {
                var eventRequest = new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    TimeOfRequestSubmition = DateTime.Now,
                    Email = email,
                    EventType = eventType,
                    StartTime = startTime,
                    EndTime = endTime,
                    MaximumGroupSize = maximumGroupSize
                };

                var groupResult = _groupMatchingController.MatchGroupFromEventRequests(eventRequest, _existingEventRequests);

                if (groupResult == null)
                {
                    // Group not found, add to list of requests that are waiting then send response.
                    _existingEventRequests.Add(eventRequest);

                    var response = this.Request.CreateResponse(HttpStatusCode.Created);
                    var responseBody = "No group found for " + eventRequest.Email + ". Added request to waiting pool.";
                    response.Content = new StringContent(responseBody, Encoding.UTF8);

                    return response;
                }
                else
                {
                    // Group was found! Remove the requests that have been grouped, send emails, and send response.
                    foreach (var request in groupResult.EventRequests)
                    {
                        if (_existingEventRequests.Contains(request))
                        {
                            _existingEventRequests.Remove(request);
                        }
                    }

                    //TODO: send email

                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    var emailsInGroup = string.Join(",", groupResult.EventRequests.Select(request => request.Email));
                    var responseBody = "Group was formed!" + Environment.NewLine
                                       + "Emails in group: " + emailsInGroup + Environment.NewLine
                                       + "Event Type: " + groupResult.EventType + Environment.NewLine
                                       + "Start Time: " + groupResult.StartTime + Environment.NewLine
                                       + "End Time: " + groupResult.EndTime;
                    response.Content = new StringContent(responseBody, Encoding.UTF8);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                return response;
            }
        }
    }
}