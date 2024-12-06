using StoreManagement.Models;
using System.Windows;

namespace StoreManagement.Services
{
    public interface IViewFactory
    {
        ViewDescriptor CreateView(string viewName);
        Window CreateView(string viewName, object parameter);
    }
}
