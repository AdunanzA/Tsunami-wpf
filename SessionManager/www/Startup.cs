using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using System.Web.Http;
using System.Collections.Generic;
using Microsoft.Owin.StaticFiles.ContentTypes;

//[assembly: OwinStartup(typeof(Tsunami.www.Startup))]

namespace Tsunami.www
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif

            // Remap '/' to '.\www\'.
            // Turns on static files and default files.
            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = PathString.Empty,
                FileSystem = new PhysicalFileSystem(@".\www"),
                EnableDefaultFiles = true,
                EnableDirectoryBrowsing = false
            });

            // Only serve files requested by name in www.
            app.UseStaticFiles("/www");

            HttpConfiguration config = new HttpConfiguration();

            //  Enable attribute based routing
            //  http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            config.MapHttpAttributeRoutes();

            // Web API
            config.Routes.MapHttpRoute(
                name: "api", 
                routeTemplate: "api/{controller}/{sha1}", 
                defaults: new { sha1 = RouteParameter.Optional}
            );
            app.UseWebApi(config);

            // Anything not handled will land at the welcome page.
            //app.UseWelcomePage();

            // signalR
            app.MapSignalR();

        }
    }
}
