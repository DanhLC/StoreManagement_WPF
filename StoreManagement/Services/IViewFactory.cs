using StoreManagement.Models;

namespace StoreManagement.Services
{
    public interface IViewFactory
    {
        ViewDescriptor CreateView(string viewName);
    }
}
