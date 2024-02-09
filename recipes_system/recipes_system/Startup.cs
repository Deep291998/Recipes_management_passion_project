using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(recipes_system.Startup))]
namespace recipes_system
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
