using FontAwesome.Sharp;

namespace StoreManagement.Models
{
    public class ViewDescriptor
    {
        public string Caption { get; }
        public IconChar Icon { get; }
        public Func<object> ViewFactory { get; }

        public ViewDescriptor(
            string caption,
            IconChar icon,
            Func<object> viewFactory)
        {
            Caption = caption;
            Icon = icon;
            ViewFactory = viewFactory;
        }
    }
}
