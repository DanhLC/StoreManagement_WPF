using FontAwesome.Sharp;
using Microsoft.Extensions.DependencyInjection;
using StoreManagement.Models;
using StoreManagement.Views;
using System.Windows;

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

        /// <summary>
        /// Create view
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Window CreateView(string viewName, object parameter)
        {
            var window = viewName switch
            {
                "CustomerInput" => new Window
                {
                    Content = _serviceProvider.GetService<CustomerInputView>(),
                    DataContext = parameter
                },
                _ => throw new ArgumentException($"View {viewName} is not registered.")
            };

            return window;
        }
    }
}
