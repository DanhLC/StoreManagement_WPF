namespace StoreManagement.ViewModels
{
    public interface IViewFactory
    {
        object CreateView(string viewName);
    }
}
