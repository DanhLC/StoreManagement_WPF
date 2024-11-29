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
        public static ServiceProvider ServiceProvider { get; private set; }

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
                loginView.Show();
            }

            base.OnStartup(e);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<UserViewModel>();

            services.AddTransient<LoginView>();
            services.AddTransient<MainFormView>();
        }
    }
}
