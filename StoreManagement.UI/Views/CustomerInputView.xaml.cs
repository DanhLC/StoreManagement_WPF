using StoreManagement.Core.Interfaces.Formatting;
using StoreManagement.Core.Interfaces.Views;
using StoreManagement.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StoreManagement.UI.Views
{
    /// <summary>
    /// Interaction logic for CustomerInputView.xaml
    /// </summary>
    public partial class CustomerInputView : UserControl, ICustomerInputView
    {
        private readonly IFormatService _formatService;
        private readonly CustomerInputViewModel _viewModel;
        private readonly CustomerViewModel _parentViewModel;

        public CustomerInputView(
            IFormatService formatService,
            CustomerInputViewModel viewModel,
            CustomerViewModel parentViewModel)
        {
            InitializeComponent();
            _formatService = formatService;
            _viewModel = viewModel;
            _parentViewModel = parentViewModel;

            DataContext = _viewModel;

            _viewModel.ShowMessage -= ShowMessageHandler;
            _viewModel.ShowMessage += ShowMessageHandler;
            _viewModel.ShowMessageConfirm -= ShowMessageConfirmHandler;
            _viewModel.ShowMessageConfirm += ShowMessageConfirmHandler;

            void ShowMessageHandler(string title, string message)
            {
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            bool ShowMessageConfirmHandler(string title, string message)
            {
                var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                return result == MessageBoxResult.Yes;
            }

            _viewModel.RequestClose += OnRequestClose;
            _viewModel.GoToHomePageRequested += OnGoToHomePageRequested;
        }

        private void OnGoToHomePageRequested()
        {
            _parentViewModel.GoToHomePageCommand.Execute(null);
        }

        private void OnRequestClose()
        {
            var parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.RequestClose -= OnRequestClose;
            _viewModel.GoToHomePageRequested -= OnGoToHomePageRequested;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var parentWindow = Window.GetWindow(this);
                parentWindow?.DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var currentWindow = Window.GetWindow(this);
            currentWindow?.Close();
        }

        private void tbDebtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var cursorPosition = textBox.CaretIndex;
            var originalText = textBox.Text;
            var convertPrice = _formatService.FormatToCurrency(
                _formatService.ParseCurrencyFormat(originalText));

            if (originalText.Contains("..") || originalText.Contains(",,"))
            {
                var correctText = originalText.Replace("..", ".").Replace(",,", ",");
                var lengthDifference = correctText.Length - originalText.Length;

                if (lengthDifference < 1) lengthDifference = 0;

                textBox.Text = correctText;
                textBox.CaretIndex = cursorPosition + lengthDifference;

                return;
            }

            if (originalText.EndsWith(",") || originalText.EndsWith(".")) return;

            var difference = convertPrice.Length - originalText.Length;
            textBox.Text = convertPrice;

            if (difference < 1) difference = 0;

            textBox.CaretIndex = cursorPosition + difference;
        }
    }
}
