using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using StatIt.Engine.Web.Services.Abstract;
using StatIt.Engine.Web.Services.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Web.Config
{
    public class WebInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .Register(Component.For<IWebRequestService>().ImplementedBy<WebRequestService>());




            //container.Register(Classes.FromThisAssembly()
            //    .InNamespace("StatIt.Engine")
            //        .WithService.DefaultInterfaces()
            //        .LifestyleTransient());

        }
    }
}
