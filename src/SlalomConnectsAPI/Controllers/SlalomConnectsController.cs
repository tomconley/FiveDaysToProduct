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
        private static List<EventGroup> _existingEventGroups;
        private static GroupController _groupController;

        static SlalomConnectsController()
        {
            _existingEventRequests = new List<EventRequest>();
            _existingEventGroups = new List<EventGroup>();
            _groupController = new GroupController();
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
        public HttpResponseMessage GetRequests()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var responseBody = _existingEventRequests.Aggregate("", (current, request) => current + request.ToString() + Environment.NewLine);
            response.Content = new StringContent(responseBody, Encoding.UTF8);

            return response;
        }

        [HttpGet]
        [Route("slalom-connects-api/get-groups")]
        public HttpResponseMessage GetGroups()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var responseBody = _existingEventGroups.Aggregate("", (current, group) => current + group.ToString() + Environment.NewLine);
            response.Content = new StringContent(responseBody, Encoding.UTF8);

            return response;
        }

        // Used for populating test data
        [HttpPost]
        [Route("slalom-connects-api/populate-test-users")]
        public void PostTestData()
        {
            _existingEventRequests.Add(new EventRequest()
            {
                EventRequestGuid = Guid.NewGuid(),
                TimeOfRequestSubmition = DateTime.Now,
                Email = "cody@slalom.com",
                EventType = EventType.Lunch,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(5)
            });

            _existingEventRequests.Add(new EventRequest()
            {
                EventRequestGuid = Guid.NewGuid(),
                TimeOfRequestSubmition = DateTime.Now,
                Email = "tom@slalom.com",
                EventType = EventType.Lunch,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2)
            });

            _existingEventRequests.Add(new EventRequest()
            {
                EventRequestGuid = Guid.NewGuid(),
                TimeOfRequestSubmition = DateTime.Now,
                Email = "brian@slalom.com",
                EventType = EventType.Lunch,
                StartTime = DateTime.Now.AddHours(3),
                EndTime = DateTime.Now.AddHours(4)
            });

            _existingEventRequests.Add(new EventRequest()
            {
                EventRequestGuid = Guid.NewGuid(),
                TimeOfRequestSubmition = DateTime.Now,
                Email = "josh@slalom.com",
                EventType = EventType.Coffee,
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(3)
            });

            _existingEventRequests.Add(new EventRequest()
            {
                EventRequestGuid = Guid.NewGuid(),
                TimeOfRequestSubmition = DateTime.Now,
                Email = "tim@slalom.com",
                EventType = EventType.Lunch,
                StartTime = DateTime.Now.AddHours(2),
                EndTime = DateTime.Now.AddHours(3)
            });
        }

        // The main entry point
        [HttpPost]
        [Route("slalom-connects-api/post-event-request")]
        public HttpResponseMessage Post(string email, EventType eventType, DateTime startTime, DateTime endTime, int? maximumGroupSize)
        {
            CheckCallerId();

            try
            {
                // Create Request from the inputs
                var newEventRequest = new EventRequest()
                {
                    EventRequestGuid = Guid.NewGuid(),
                    TimeOfRequestSubmition = DateTime.Now,
                    Email = email,
                    EventType = eventType,
                    StartTime = startTime,
                    EndTime = endTime,
                    MaximumGroupSize = maximumGroupSize
                };

                if (_existingEventGroups.Count > 0)
                {
                    _groupController.RemoveGroupsAtOrPastTheirStartTime(_existingEventGroups);
                }

                #region Existing group check

                // Checks for an existing group that could add the new request
                var existingGroupThatCanAddNewRequest =
                    _groupController.CheckForGroupThatCouldAddTheNewEventRequest(newEventRequest, _existingEventGroups);

                if (existingGroupThatCanAddNewRequest != null)
                {
                    existingGroupThatCanAddNewRequest.EventRequests.Add(newEventRequest);

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    var emailsInGroup = string.Join(",", existingGroupThatCanAddNewRequest.EventRequests.Select(request => request.Email));
                    var responseBody = "Group was formed!" + Environment.NewLine
                                       + "Emails in group: " + emailsInGroup + Environment.NewLine
                                       + "Event Type: " + existingGroupThatCanAddNewRequest.EventType + Environment.NewLine
                                       + "Start Time: " + existingGroupThatCanAddNewRequest.StartTime + Environment.NewLine
                                       + "End Time: " + existingGroupThatCanAddNewRequest.EndTime;
                    response.Content = new StringContent(responseBody, Encoding.UTF8);

                    return response;
                }

                #endregion Existing group check

                #region New group check

                // No existing group, check if we can make a new group from those waiting
                var groupResult = _groupController.MatchGroupFromEventRequests(newEventRequest, _existingEventRequests);

                // Evaluates result of grouping
                if (groupResult == null)
                {
                    // Group not found, add to list of requests that are waiting then send response.

                    _existingEventRequests.Add(newEventRequest);

                    var response = Request.CreateResponse(HttpStatusCode.Created);
                    var responseBody = "No group found for " + newEventRequest.Email + ". Added request to waiting pool.";
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

                    _existingEventGroups.Add(groupResult);

                    //TODO: send email

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    var emailsInGroup = string.Join(",", groupResult.EventRequests.Select(request => request.Email));
                    var responseBody = "Group was formed!" + Environment.NewLine
                                       + "Emails in group: " + emailsInGroup + Environment.NewLine
                                       + "Event Type: " + groupResult.EventType + Environment.NewLine
                                       + "Start Time: " + groupResult.StartTime + Environment.NewLine
                                       + "End Time: " + groupResult.EndTime;
                    response.Content = new StringContent(responseBody, Encoding.UTF8);

                    return response;
                }

                #endregion New group check
            }
            catch (Exception ex)
            {
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                return response;
            }
        }
    }
}