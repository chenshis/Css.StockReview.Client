using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Infrastructure.Config.HttpClients
{
    /// <summary>
    /// 服务注入
    /// </summary>
    public static class PrismServiceProviderExtensions
    {
        /// <summary>
        /// 创建服务提供器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="actionServices"></param>
        public static void CreateServiceProvider(this IContainerRegistry container, Action<IServiceCollection> actionServices)
        {
            IServiceCollection services = new ServiceCollection();
            actionServices(services);
            RegisterTypesWithPrismContainer(container, services);
            var serviceProvider = services.BuildServiceProvider();
            if (!IContainerRegistryExtensions.IsRegistered<IServiceProvider>(container))
            {
                container.RegisterInstance<IServiceProvider>(serviceProvider);
            }
        }

        internal static void RegisterTypesWithPrismContainer(IContainerRegistry containerRegistry, IServiceCollection services)
        {
            foreach (ServiceDescriptor service in services)
            {
                switch (service.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        if (service.ImplementationType != null)
                        {
                            containerRegistry.RegisterSingleton(service.ServiceType, service.ImplementationType);
                        }
                        else if (service.ImplementationInstance != null)
                        {
                            containerRegistry.RegisterInstance(service.ServiceType, service.ImplementationInstance);
                        }
                        else if (service.ImplementationFactory != null)
                        {
                            containerRegistry.RegisterSingleton(service.ServiceType, (IContainerProvider c) => ServiceFactory(c, service.ImplementationFactory));
                        }

                        break;
                    case ServiceLifetime.Transient:
                        if (service.ImplementationType != null)
                        {
                            containerRegistry.Register(service.ServiceType, service.ImplementationType);
                        }
                        else if (service.ImplementationFactory != null)
                        {
                            containerRegistry.Register(service.ServiceType, (IContainerProvider c) => ServiceFactory(c, service.ImplementationFactory));
                        }

                        break;
                    case ServiceLifetime.Scoped:
                        if (service.ImplementationType != null)
                        {
                            containerRegistry.RegisterScoped(service.ServiceType, service.ImplementationType);
                            break;
                        }

                        if ((object)service.ImplementationType == null)
                        {
                            containerRegistry.RegisterScoped(service.ServiceType, (IContainerProvider c) => ServiceFactory(c, service.ImplementationFactory));
                            break;
                        }

                        if (service.ServiceType.IsAbstract)
                        {
                            throw new NotSupportedException("Cannot register the service " + service.ServiceType.FullName + " as it is an abstract type");
                        }

                        if (service.ServiceType.IsInterface)
                        {
                            throw new NotSupportedException("Cannot register the service " + service.ServiceType.FullName + " as it is an interface. You must provide a concrete implementation");
                        }

                        containerRegistry.RegisterScoped(service.ServiceType);
                        break;
                }
            }
        }

        private static object ServiceFactory(IContainerProvider container, Func<IServiceProvider, object> implementationFactory)
        {
            IServiceProvider arg = container.Resolve<IServiceProvider>();
            return implementationFactory(arg);
        }
    }
}
