using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SlalomConnectsAPI.Models;

namespace SlalomConnectsAPI.Controllers
{
    public class SlalomConnectsController : ApiController
    {
        // Uncomment following lines for service principal authentication
        //private static string trustedCallerClientId = ConfigurationManager.AppSettings["todo:TrustedCallerClientId"];
        //private static string trustedCallerServicePrincipalId = ConfigurationManager.AppSettings["todo:TrustedCallerServicePrincipalId"];

        //private static Dictionary<int, ToDoItem> mockData = new Dictionary<int, ToDoItem>();

        static SlalomConnectsController()
        {
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

        // POST: api/SlalomConnects
        [HttpPost]
        [Route("api/slalom_connects/new_event_request")]
        public void Post(string email, EventType eventType, DateTime startTime, DateTime endTime)
        {
            CheckCallerId();

            //todo.ID = mockData.Count > 0 ? mockData.Keys.Max() + 1 : 1;
            //mockData.Add(todo.ID, todo);
        }
    }
}