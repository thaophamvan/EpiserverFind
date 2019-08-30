using System;
using System.Web.Mvc;
using Core.Querying.Infrastructure.Ioc;
using Core.Querying.Infrastructure.ProtectedCall;
using Core.Querying.Services;
using Core.Querying.Shared;
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
                context.Services.AddTransient<IContentDataQuery<IContent>, ContentDataQuery<IContent>>();
                context.Services.AddSingleton<IContentDataQueryHandler, ContentDataQueryHandler>();
                context.Services.AddSingleton<ICircuitBreaker, CircuitBreaker>();
                
                context.Services.AddTransient<IMultipleSearchProvider, IMultipleSearchProvider>();
                context.Services.AddTransient<EpiserverFindServices>();
                context.Services.AddTransient<CacheSearchServices>();
                context.Services.AddTransient<LuceneSearchServices>();
                context.Services.AddTransient<DatabaseSearchServices>();


                context.Services.AddTransient<Func<ServiceEnum, ISearchServices>>(serviceProvider => key =>
                {
                    switch (key)
                    {
                        case ServiceEnum.Find:
                            return serviceProvider.GetInstance<EpiserverFindServices>();
                        case ServiceEnum.Cache:
                            return serviceProvider.GetInstance<CacheSearchServices>();
                        case ServiceEnum.Lucene:
                            return serviceProvider.GetInstance<LuceneSearchServices>();
                        case ServiceEnum.Database:
                            return serviceProvider.GetInstance<DatabaseSearchServices>();
                        default:
                            return null;
                    }
                });


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
