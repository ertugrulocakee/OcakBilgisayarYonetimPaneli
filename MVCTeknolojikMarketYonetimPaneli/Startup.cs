using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCTeknolojikMarketYonetimPaneli.Startup))]
namespace MVCTeknolojikMarketYonetimPaneli
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
