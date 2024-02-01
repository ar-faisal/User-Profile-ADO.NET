using ADO.NET.Application.Common.Interfaces;
using ADO.NET.Infrastructure.Repository;
using System;
using System.Configuration;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace ADO.NET.WebApp
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            string connectionString = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
            container.RegisterInstance<IDbContext>(new AdoDbContext(connectionString));


            container.RegisterType<IUnitOfWork, UnitOfWork>();
        }

        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}