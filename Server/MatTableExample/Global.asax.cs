using System.Web;
using System.Web.Http;
using System.Web.Mvc;

using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MatTableExample
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
#if DEBUG
            json.SerializerSettings.Formatting = Formatting.Indented;
#else 
            json.SerializerSettings.Formatting = Formatting.None;
#endif
            // on renvoit le json en camelCase
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // on fait gaffe aux problèmes de références circulaires
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
