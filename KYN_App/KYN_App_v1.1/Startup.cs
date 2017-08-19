using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KYN_App_v1._1.Startup))]
namespace KYN_App_v1._1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
