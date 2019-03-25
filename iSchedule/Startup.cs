using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iSchedule.Startup))]
namespace iSchedule
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
