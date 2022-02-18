using System.Web.Mvc;
using System.Web.Routing;

namespace FinalEmarketingProjectMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }
    }
}
