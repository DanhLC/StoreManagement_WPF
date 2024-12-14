using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreManagement.Core.Interfaces.Formatting;
using StoreManagement.Core.Interfaces.Services;
using StoreManagement.Core.Interfaces.Views;
using StoreManagement.Infrastructure.Data;
using StoreManagement.Services;
using StoreManagement.Services.Formatting;
using StoreManagement.UI.ViewModels;
using StoreManagement.UI.Views;
using System.Globalization;
using System.Windows;

namespace StoreManagement.UI
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

            services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=Data/StoreManagement.db"));
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IConfigRepository, ConfigRepository>();

            #endregion

            #region Scoped (View model)

            services.AddScoped<UserViewModel>();
            services.AddScoped<MainViewModel>();
            services.AddScoped<DashboardViewModel>();
            services.AddScoped<OrderViewModel>();
            services.AddScoped<CustomerViewModel>();
            services.AddScoped<CustomerInputViewModel>();

            #endregion

            #region Singleton

            services.AddSingleton<IViewFactory, ViewFactory>();
            services.AddSingleton<ISessionManager>(SessionManager.Instance);
            services.AddSingleton<IFormatService, FormatService>(provider => new FormatService(new CultureInfo("vi-VN")));

            #endregion

            #region Transient (View)

            services.AddTransient<LoginView>();
            services.AddTransient<MainFormView>();
            services.AddTransient<IDashboardView, DashboardView>();
            services.AddTransient<IOrderView, OrderView>();
            services.AddTransient<ICustomerView, CustomerView>();
            services.AddTransient<ICustomerInputView, CustomerInputView>();

            #endregion
        }
    }
}
