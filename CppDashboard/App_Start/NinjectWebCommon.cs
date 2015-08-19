using System.Web.Http;
using System.Web.Http.Dependencies;
using CppDashboard.Controllers;
using CppDashboard.DataProvider;
using CppDashboard.Initialisers;
using CppDashboard.Logic;
using CppDashboard.Logic.General;
using CppDashboard.Logic.Offline;
using CppDashboard.Logic.Orphans;
using CppDashboard.Logic.Payments;
using CppDashboard.Logic.Refusals;
using CppDashboard.Models;
using Ninject.Parameters;
using Ninject.Syntax;
using Ninject.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CppDashboard.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(CppDashboard.App_Start.NinjectWebCommon), "Stop")]

namespace CppDashboard.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<PageModelBuilder>().To<PageModelBuilder>();
            kernel.Bind<ICancellationsDueToOrphan>().To<CancellationsDueToOrphan>();
            kernel.Bind<IPaymentsCalculator>().To<PaymentsCalculator>();
            kernel.Bind<IGatewayRefusals>().To<GatewayRefusals>();
            kernel.Bind<SystemOnlineOrOfflineStatus>().ToSelf();
            
            
            // System data tab loads everytime.
            kernel.Bind<IErrorSummaryWindow, ILoadSystemData>().To<ErrorSummaryWindow>().InSingletonScope();
            kernel.Bind<ISystemEventSummaryWindow, ILoadSystemData>().To<SystemEventSummaryWindow>().InSingletonScope();


            // Volatile data that required to be refreshed regulaly.
            kernel.Bind<ILoggingInfo, ICanReload, ILoadVolatileData>().To<LoggingDataCanFacade>().InSingletonScope();
            kernel.Bind<IMonitoringEvents, ICanReload, ILoadVolatileData>().To<PaymentEventsDataCanFacade>().InSingletonScope();
            kernel.Bind<IPaymentInfo, ICanReload, ILoadVolatileData>().To<PaymentsDataCanFacade>().InSingletonScope();
            kernel.Bind<IOfflineConfigs, ICanReload, ILoadVolatileData>().To<OfflineDataFacade>().InSingletonScope();

            kernel.Bind<IInitialiser>()
                .To<ShortTermDataInitialiser>()
                .WhenInjectedInto<DashboardDataController>()
                .InSingletonScope();

            kernel.Bind<IInitialiser>()
                .To<SystemDataInitialiser>()
                .WhenInjectedInto<SystemDataController>()
                .InSingletonScope();
        }
    }

    // Provides a Ninject implementation of IDependencyScope
    // which resolves services using the Ninject container.
    public class NinjectDependencyScope : IDependencyScope
    {
        IResolutionRoot resolver;

        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            return resolver.TryGet(serviceType);
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has been disposed");

            return resolver.GetAll(serviceType);
        }

        public void Dispose()
        {
            //IDisposable disposable = resolver as IDisposable;
            //if (disposable != null)
            //    disposable.Dispose();

            //resolver = null;
        }
    }

    // This class is the resolver, but it is also the global scope
    // so we derive from NinjectScope.
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel);
        }
    }
}
