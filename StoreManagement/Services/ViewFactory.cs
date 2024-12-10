using FontAwesome.Sharp;
using Microsoft.Extensions.DependencyInjection;
using StoreManagement.Models;
using StoreManagement.Views;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StoreManagement.Services
{
    public class ViewFactory : IViewFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create child form view (User control)
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public ViewDescriptor CreateView(string viewName)
        {
            return GetView(viewName) ?? throw new ArgumentException($"View {viewName} is not supported.");
        }

        private ViewDescriptor? GetView(string viewName)
        {
            return viewName switch
            {
                "Dashboard" => new ViewDescriptor(
                    caption: "Dashboard",
                    icon: IconChar.Home,
                    viewFactory: () => _serviceProvider.GetService<DashboardView>()!
                ),
                "Order" => new ViewDescriptor(
                    caption: "Orders",
                    icon: IconChar.Truck,
                    viewFactory: () => _serviceProvider.GetService<OrderView>()!
                ),
                "Customer" => new ViewDescriptor(
                    caption: "Customers",
                    icon: IconChar.UserGroup,
                    viewFactory: () => _serviceProvider.GetService<CustomerView>()!
                ),
                _ => null
            };
        }

        public void OpenViewInput(string viewName, object parameter, int height, int width, string title)
        {
            var content = viewName switch
            {
                "CustomerInput" => _serviceProvider.GetService<CustomerInputView>(),
                _ => throw new ArgumentException($"View {viewName} is not registered.")
            };

            var window = new Window
            {
                Content = content,
                DataContext = parameter,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Height = height,
                Width = width,
                Background = Brushes.Transparent,
                AllowsTransparency = true,
                Title = title,
                Icon = new BitmapImage(new Uri("pack://application:,,,/Images/app_icon.ico"))
            };

            window.ShowDialog();
        }
    }
}
