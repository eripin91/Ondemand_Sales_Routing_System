using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace iSchedule
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}"
            );

            /* if TRUE POST is to be used, then the following will have to be commented out. */

            ////Clear current formatters
            //config.Formatters.Clear();

            ////Add only a json formatter
            //config.Formatters.Add(new TextMediaTypeFormatter());
        }
    }
}
