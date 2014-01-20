using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using StatIt.Engine.Distimo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatIt.Engine.Distimo.Config
{
    public class DistimoInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(Classes.FromThisAssembly()
                .InNamespace("StatIt.Engine.Distimo.Services")
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());

        }
    }
}
