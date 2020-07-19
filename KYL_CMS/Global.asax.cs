using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KYL_CMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            try
            {
                if (Session["ID"] != null)
                {
                    KYL_CMS.Models.HelpLibrary.Log.Write("Global", "Session_Start SessionID:" + Session.SessionID);
                    KYL_CMS.Models.HelpLibrary.Log.Write("Global", "Session_Start Session[ID]:" + Session["ID"].ToString());
                }
            }
            catch (Exception ex)
            {
            }
       
        }

        protected void Session_End()
        {
            try
            {
                if (Session["ID"] != null)
                {
                    KYL_CMS.Models.HelpLibrary.Log.Write("Global", "Session_End SessionID:" + Session.SessionID);
                    KYL_CMS.Models.HelpLibrary.Log.Write("Global", "Session_End Session[ID]:" + Session["ID"].ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
