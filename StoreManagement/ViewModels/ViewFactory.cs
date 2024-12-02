using Microsoft.Extensions.DependencyInjection;
using StoreManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return viewName switch
            {
                "Dashboard" => _serviceProvider.GetService<DashboardView>(),
                "Order" => _serviceProvider.GetService<OrderView>(),
                _ => throw new ArgumentException($"View {viewName} is not supported")
            };
        }
    }
}
