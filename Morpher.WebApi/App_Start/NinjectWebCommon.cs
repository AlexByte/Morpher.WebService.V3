[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Morpher.WebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Morpher.WebApi.App_Start.NinjectWebCommon), "Stop")]

namespace Morpher.WebApi.App_Start
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Http;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Services;
    using Morpher.WebApi.Services.Interfaces;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.WebApi;

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

            bool IsLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]);

            if (IsLocal)
            {
                kernel.Bind<ICustomDeclensions>().To<CustomDeclensionsLocal>();
                kernel.Bind<IApiThrottler>().ToConstant(new ApiThrottlerLocal());
                kernel.Bind<IMorpherLog>().ToConstant(new MorpherLogLocal());
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MorpherDatabase"].ConnectionString;

                int cacheSize = Convert.ToInt32(ConfigurationManager.AppSettings["Cachesize"]);
                kernel.Bind<ICustomDeclensions>()
                    .To<CustomDeclensions>()
                    .WithConstructorArgument("connectionString", connectionString);
                kernel.Bind<IApiThrottler>().ToConstant(new ApiThrottler(connectionString));
                kernel.Bind<IMorpherLog>().ToConstant(new MorpherLog(connectionString, cacheSize));
            }


            kernel.Bind<IRussianAnalyzer>().To<RussianWebAnalyzer>();
            kernel.Bind<IUkrainianAnalyzer>().To<UkrainianWebAnalyzer>();
        }
    }
}
