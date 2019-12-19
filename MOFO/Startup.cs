using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MOFO.Startup))]
namespace MOFO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
