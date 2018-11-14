using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReportWebApp01.Startup))]
namespace ReportWebApp01
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
