using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using System.Web.Http;

[assembly: OwinStartup(typeof(Tsunami.Gui.Wpf.www.Startup))]

namespace Tsunami.Gui.Wpf.www
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

            // Only serve files requested by name.
            app.UseStaticFiles("/www");

            // Web API
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "api", 
                routeTemplate: "api/{controller}/{id}", 
                defaults: new { id = RouteParameter.Optional}
            );
            app.UseWebApi(config);

            // Anything not handled will land at the welcome page.
            //app.UseWelcomePage();

            // signalR
            app.MapSignalR();

        }
    }
}
