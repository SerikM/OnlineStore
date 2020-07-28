
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebUI2.Startup))]
namespace WebUI2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
