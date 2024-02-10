using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppPassionProjectMVP.Startup))]
namespace WebAppPassionProjectMVP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
