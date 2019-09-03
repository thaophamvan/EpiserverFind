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
                context.Services.AddSingleton<IContentDataQueryHandler, ContentDataQueryHandler>();
                context.Services.AddSingleton<ICircuitBreaker, CircuitBreaker>();
                
                context.Services.AddTransient<IMultipleSearchProvider, IMultipleSearchProvider>();
                context.Services.AddTransient<EpiserverFindServices<IContent>>();
                context.Services.AddTransient<CacheSearchServices<IContent>>();
                context.Services.AddTransient<LuceneSearchServices<IContent>>();
                context.Services.AddTransient<DatabaseSearchServices<IContent>>();


                context.Services.AddTransient<Func<ServiceEnum, ISearchServices<IContent>>>(serviceProvider => key =>
                {
                    switch (key)
                    {
                        case ServiceEnum.Find:
                            return serviceProvider.GetInstance<EpiserverFindServices<IContent>>();
                        case ServiceEnum.Cache:
                            return serviceProvider.GetInstance<CacheSearchServices<IContent>>();
                        case ServiceEnum.Lucene:
                            return serviceProvider.GetInstance<LuceneSearchServices<IContent>>();
                        case ServiceEnum.Database:
                            return serviceProvider.GetInstance<DatabaseSearchServices<IContent>>();
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
