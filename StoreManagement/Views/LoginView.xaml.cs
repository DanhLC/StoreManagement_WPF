using Microsoft.Extensions.DependencyInjection;
using StoreManagement.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StoreManagement.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetService<UserViewModel>();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserViewModel viewModel && sender is PasswordBox passwordBox)
            {
                //var caretPosition = passwordBox.SelectionStart;

                viewModel.PlainPassword = passwordBox.Password;
                viewModel.SecurePassword = passwordBox.Password;

                //passwordBox.Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    passwordBox.Select(caretPosition, 0);
                //})); 
            }
        }
        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is UserViewModel viewModel && sender is TextBox textBox)
            {
                var caretPosition = tbPassword.SelectionStart;

                viewModel.PlainPassword = textBox.Text;

                if (!viewModel.IsPasswordVisible && pbPassword != null)
                {
                    pbPassword.Password = textBox.Text;
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
