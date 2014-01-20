using Castle.Windsor;
using Castle.Windsor.Installer;
using StatIt.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StatIt.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BootstrapContainer();
        }

        protected void Application_End()
        {
            container.Dispose();
        }

        /// <summary>
        /// Create a new IoC Container and set the ControllerBuilder to our custom build
        /// </summary>
        private static void BootstrapContainer()
        {
            container = new WindsorContainer()
                .Install(FromAssembly.This())
                .Install(FromAssembly.Named("StatIt.Engine"));

            var controllerFactory = new StatItControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}
