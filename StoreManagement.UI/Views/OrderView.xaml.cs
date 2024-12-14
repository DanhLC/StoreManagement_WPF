using StoreManagement.Core.Interfaces.Views;
using System.Windows.Controls;

namespace StoreManagement.UI.Views
{
    /// <summary>
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : UserControl, IOrderView
    {
        public OrderView()
        {
            InitializeComponent();
        }
    }
}
