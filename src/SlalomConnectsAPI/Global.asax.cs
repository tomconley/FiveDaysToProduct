using SlalomConnectsAPI;
using System.Web.Http;

namespace ToDoListDataAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}