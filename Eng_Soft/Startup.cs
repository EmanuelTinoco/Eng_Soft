using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Eng_Soft.Startup))]
namespace Eng_Soft
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
