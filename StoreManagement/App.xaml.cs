using Microsoft.Extensions.DependencyInjection;
using StoreManagement.Data;
using StoreManagement.Services;
using StoreManagement.ViewModels;
using StoreManagement.Views;
using System.Windows;

namespace StoreManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            //ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var loginView = ServiceProvider.GetService<LoginView>();

            if (MainWindow is LoginView)
            {
                MainWindow = loginView;
                loginView?.Show();
            }

            base.OnStartup(e);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            #region DB context

            services.AddDbContext<AppDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IConfigRepository, ConfigRepository>();

            #endregion

            #region Scoped (View model)

            services.AddScoped<UserViewModel>();
            services.AddScoped<MainViewModel>();
            services.AddScoped<DashboardViewModel>();
            services.AddScoped<OrderViewModel>();
            services.AddScoped<CustomerViewModel>();

            #endregion

            #region Singleton

            services.AddSingleton<IViewFactory, ViewFactory>();
            services.AddSingleton<ISessionManager>(SessionManager.Instance);

            #endregion

            #region Transient (View)

            services.AddTransient<LoginView>();
            services.AddTransient<MainFormView>();
            services.AddTransient<DashboardView>();
            services.AddTransient<OrderView>();
            services.AddTransient<CustomerView>();

            #endregion
        }
    }
}
