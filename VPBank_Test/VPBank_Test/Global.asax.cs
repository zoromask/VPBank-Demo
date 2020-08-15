using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Common;
using ObjectInfo;
namespace VPBank_Test
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            LoadAllConfigData();
        }
      
        protected void LoadAllConfigData()
        {
            try
            {
                CommonSystem.gConnectStringSQL = ConfigurationManager.ConnectionStrings["ConnectionString_SQL"].ConnectionString;
            }
            catch (Exception)
            {
            }
        }
       
    }
}
