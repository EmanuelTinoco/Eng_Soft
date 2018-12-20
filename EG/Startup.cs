using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EG.Startup))]
namespace EG
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
