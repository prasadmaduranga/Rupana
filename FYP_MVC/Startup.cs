using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FYP_MVC.Startup))]
namespace FYP_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
