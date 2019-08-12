using System.Web.Mvc;
using Core.Querying.Infrastructure.Ioc;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using StructureMap;

namespace Core.Querying.Infrastructure.Initialization
{
    [InitializableModule]
    public class DependencyResolverInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.ConfigurationComplete += (o, e) =>
            {
                context.Services.AddTransient<IContentDataQueryFactory, ContentDataQueryFactory>();
                context.Services.AddTransient<IContentDataQuery<IContentData>, ContentDataQuery<IContentData>>();
                context.Services.AddSingleton<IContentDataQueryHandler, ContentDataQueryHandler>();
            };
        }

        private static void ConfigureContainer(ConfigurationExpression container)
        {
           
        }

        public void Initialize(InitializationEngine context)
        {
            DependencyResolver.SetResolver(new ServiceLocatorDependencyResolver(context.Locate.Advanced));
        }
        public void Uninitialize(InitializationEngine context)
        {
        }
        public void Preload(string[] parameters)
        {
        }
    }
}
