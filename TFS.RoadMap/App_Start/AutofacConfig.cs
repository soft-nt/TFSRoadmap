using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using TFS.RoadMap.Interfaces;

namespace TFS.RoadMap
{
    public class AutofacConfig
    {
        public static void RegisterAutofac()
        {
            var builder = new ContainerBuilder();
            

            // Register controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register providers
            builder.RegisterType<TfsRoadmapProvider>().As<IRoadmapProvider>();

            var container = builder.Build();

            // MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //MVC API
            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}