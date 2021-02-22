
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Globalization;
using System.Threading;
using GSD.Globalization;
using Ninject_MVC.Controllers;

namespace Koshop.web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {


            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(new NinjectController());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var persianCulture = new  PersianCulture();
            Thread.CurrentThread.CurrentCulture = persianCulture;
            Thread.CurrentThread.CurrentUICulture = persianCulture;
            persianCulture.NumberFormat.NumberDecimalSeparator = ".";
            persianCulture.NumberFormat.CurrencySymbol = "";
            persianCulture.NumberFormat.CurrencyDecimalDigits = 0;
        }

        protected void Application_PostAuthorizeRequest()
        {
            //Allow Read Value Session In WebApi
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            if(!Request.IsAuthenticated && new HttpRequestWrapper(Request).IsAjaxRequest())
            {
                //Response.Redirect(Request.RawUrl);
            }

        }
        protected void Application_EndRequest()
        {
            // redirected to the login page.
            var context = new HttpContextWrapper(Context);
            if (context.Response.StatusCode == 302 && context.Request.IsAjaxRequest() && context.Response.RedirectLocation.StartsWith("/Account/Login"))
            {
                context.Response.Clear();
                Context.Response.StatusCode = 401;
            }
        }
    
    protected void Application_PreAuthorizeRequest()
        {

        }

        //protected void Application_AuthenticateRequest(object sender,EventArgs e)
        //{

        //}

    }
}