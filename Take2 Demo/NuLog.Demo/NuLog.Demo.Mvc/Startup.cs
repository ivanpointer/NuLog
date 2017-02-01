using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NuLog.Demo.Mvc.Startup))]
namespace NuLog.Demo.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
