using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using Troy.Data.DataContext;
using Troy.Data.Repository;
using Troy.Model.AppMembership;
using Troy.Web.Controllers;
using Troy.Data.Repository.MasterData;

namespace Troy.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<IPurchaseRepository, PurchaseRepository>();
            container.RegisterType<IManufacturerRepository, ManufactureRepository>();
            container.RegisterType<IConfigurationRepository,ConfigurationRepository>();
            container.RegisterType<IBranchRepository, BranchRepository>();
            container.RegisterType<IProductGroupRepository, ProductGroupRepository>();
            container.RegisterType<IBusinessPartnerRepository, BusinessPartnerRepository>();
            container.RegisterType<IEmployeeRepository, EmployeeRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IGoodsReceiptRepository,GoodsReceiptRepository>();
            container.RegisterType<IYearRepository, YearRepository>();
            container.RegisterType<IPurchaseOrderRepository, PurchaseOrderRepository>();
            container.RegisterType<IPurchaseReturnRepository, PurchaseReturnRepository>();

            container.RegisterType(typeof(UserManager<>), new InjectionConstructor(typeof(IUserStore<>)));
            container.RegisterType<IUser>(new InjectionFactory(c => c.Resolve<Microsoft.AspNet.Identity.IUser>()));
            container.RegisterType(typeof(IUserStore<>), typeof(UserStore<>));
            container.RegisterType<IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, ApplicationUser>(new ContainerControlledLifetimeManager());
            container.RegisterType<DbContext, ApplicationDbContext>(new ContainerControlledLifetimeManager());

            container.RegisterType<UserManager<ApplicationUser, int>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser, int>, UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>>(new HierarchicalLifetimeManager());
            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());

            container.RegisterType<AccountController>(new InjectionConstructor(new object[] { typeof(IBranchRepository), typeof(IYearRepository) }));
        }
    }
}
