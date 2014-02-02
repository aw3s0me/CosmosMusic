using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Web.Security;
using System.Security.Principal;
using System.Threading;

namespace CosmosMusic
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            if (context.Request.IsAuthenticated)
            {
                string[] roles = LookupRolesForUser(context.User.Identity.Name);
                var newUser = new GenericPrincipal(context.User.Identity, roles);
                context.User = Thread.CurrentPrincipal = newUser;
            }
        }

        #region helper

        private string[] LookupRolesForUser(string userName)
        {
            string[] roles = new string[1];
            CosmosMusic.Models.korovin_idzEntities db = new CosmosMusic.Models.korovin_idzEntities();
            var roleId = db.Users.Where(x => x.username == userName).FirstOrDefault().id_role;
            roles[0] = db.Role.Where(y => y.id_role == roleId).FirstOrDefault().name;

            return roles;
        }
        #endregion

    }
}