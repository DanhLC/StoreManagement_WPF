using FontAwesome.Sharp;
using StoreManagement.Core;
using StoreManagement.Core.Interfaces.Services;
using StoreManagement.Core.Interfaces.Views;
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
                    viewFactory: () => (IDashboardView)_serviceProvider.GetService(typeof(IDashboardView))!
                ),
                "Order" => new ViewDescriptor(
                    caption: "Orders",
                    icon: IconChar.Truck,
                    viewFactory: () => (IOrderView)_serviceProvider.GetService(typeof(IOrderView))!
                ),
                "Customer" => new ViewDescriptor(
                    caption: "Customers",
                    icon: IconChar.UserGroup,
                    viewFactory: () => (ICustomerView)_serviceProvider.GetService(typeof(ICustomerView))!
                ),
                "User" => new ViewDescriptor(
                    caption: "Users",
                    icon: IconChar.UserCheck,
                    viewFactory: () => (IUserView)_serviceProvider.GetService(typeof(IUserView))!
                ),
                _ => null
            };
        }

        public void OpenViewInput(string viewName, object parameter, int height, int width, string title)
        {
            var content = viewName switch
            {
                "CustomerInput" => (ICustomerInputView)_serviceProvider.GetService(typeof(ICustomerInputView))!,
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
