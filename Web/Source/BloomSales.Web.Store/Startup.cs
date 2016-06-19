using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BloomSales.Web.Store.Startup))]
namespace BloomSales.Web.Store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
