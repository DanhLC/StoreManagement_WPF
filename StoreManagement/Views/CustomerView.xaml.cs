using Microsoft.Extensions.DependencyInjection;
using StoreManagement.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StoreManagement.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();

            var viewModel = App.ServiceProvider.GetService<CustomerViewModel>();
            DataContext = viewModel;

            viewModel.ShowMessageConfirm += (title, message) =>
            {
                var result = MessageBox.Show(
                     message,
                     title,
                     MessageBoxButton.YesNo,
                     MessageBoxImage.Question
                 );

                return result == MessageBoxResult.Yes;
            };

            viewModel.ShowMessage += (title, message) =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            };

            if (viewModel != null)
            {
                Loaded += async (s, e) => await viewModel.LoadPageAsync(1);
            }
        }

        private double CalculateAvailableHeight(DataGrid dataGrid, double headerHeight)
        {
            var padding = dataGrid.Padding.Top + dataGrid.Padding.Bottom;
            var margin = dataGrid.Margin.Top + dataGrid.Margin.Bottom;
            var nonDataHeight = headerHeight + padding + margin;

            return Math.Max(0, dataGrid.ActualHeight - nonDataHeight);
        }

        private void DataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is CustomerViewModel viewModel)
            {
                const double rowHeight = 41;
                const double headerHeight = 30;
                var dataGrid = sender as DataGrid;

                if (dataGrid != null)
                {
                    var availableHeight = CalculateAvailableHeight(dataGrid, headerHeight);
                    var rows = Math.Max((int)Math.Floor(availableHeight / rowHeight), 1);

                    viewModel.UpdatePageSize(rows);
                    viewModel.LoadPageCommand.Execute(null);
                }
            }
        }

        private void tbGoto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out int result);
        }
    }
}