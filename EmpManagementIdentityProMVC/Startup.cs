using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmpManagementIdentityProMVC.Startup))]
namespace EmpManagementIdentityProMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
