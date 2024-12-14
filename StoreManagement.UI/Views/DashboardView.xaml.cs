using StoreManagement.Core.Interfaces.Views;
using System.Windows.Controls;

namespace StoreManagement.UI.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl, IDashboardView
    {
        public DashboardView()
        {
            InitializeComponent();
        }
    }
}
