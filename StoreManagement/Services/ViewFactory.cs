using FontAwesome.Sharp;
using Microsoft.Extensions.DependencyInjection;
using StoreManagement.Models;
using StoreManagement.Views;

namespace StoreManagement.Services
{
    public class ViewFactory : IViewFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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
    }
}
