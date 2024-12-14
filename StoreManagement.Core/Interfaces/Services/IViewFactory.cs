namespace StoreManagement.Core.Interfaces.Services
{
    public interface IViewFactory
    {
        ViewDescriptor CreateView(string viewName);
        void OpenViewInput(string viewName, object parameter, int height, int width, string title);
    }
}
