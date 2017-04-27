using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VictoriasFood.Startup))]
namespace VictoriasFood
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
