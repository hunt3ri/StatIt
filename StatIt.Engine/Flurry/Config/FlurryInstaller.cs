using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Flurry.Config
{
    public class FlurryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(Classes.FromThisAssembly()
                .InNamespace("StatIt.Engine.Flurry.Services")
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());

        }
    }
}
