using Microsoft.Extensions.DependencyInjection;
using StoreManagement.Views;

namespace StoreManagement.ViewModels
{
    public class ViewFactory : IViewFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object CreateView(string viewName)
        {
            return GetView(viewName) ?? throw new ArgumentException($"View {viewName} is not supported.");
        }

        private object? GetView(string viewName)
        {
            return viewName switch
            {
                "Dashboard" => _serviceProvider.GetService<DashboardView>(),
                "Order" => _serviceProvider.GetService<OrderView>(),
                "Customer" => _serviceProvider.GetService<CustomerView>(),
                _ => null
            };
        }
    }
}
