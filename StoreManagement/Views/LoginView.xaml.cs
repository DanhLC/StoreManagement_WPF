using Microsoft.Extensions.DependencyInjection;
using StoreManagement.Services;
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

            var userViewModel = App.ServiceProvider.GetService<UserViewModel>();
            DataContext = userViewModel;
            userViewModel.OnLoginSuccess += () =>
            {
                var mainForm = new MainFormView();
                mainForm.Show();
                Close();
            };
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserViewModel viewModel && sender is PasswordBox passwordBox)
            {
                viewModel.Password = passwordBox.Password;
                viewModel.PlainPassword = passwordBox.Password;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserViewModel viewModel)
            {
                pbPassword.Password = viewModel.PlainPassword;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserViewModel viewModel)
            {
                pbPassword.Password = viewModel.PlainPassword;
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
