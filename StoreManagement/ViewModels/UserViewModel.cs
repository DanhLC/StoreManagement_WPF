using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoreManagement.Models;
using StoreManagement.Services;
using StoreManagement.Views;
using System.Windows;

namespace StoreManagement.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        private readonly IRepository<Users> _userRepository;

        public UserViewModel(IRepository<Users> userRepository)
        {
            _userRepository = userRepository;
            IsPasswordVisible = false;
        }

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isPasswordVisible;

        [ObservableProperty]
        private string plainPassword;

        public string SecurePassword { get; set; }

        public string VisiblePassword => IsPasswordVisible ? Password : new string('*', Password?.Length ?? 0);

        public IRelayCommand LoginCommand => new AsyncRelayCommand(LoginAsync);

        private async Task LoginAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (user != null)
            {
                var mainForm = new MainFormView();
                SessionManager.Instance.UserId = user.Id;
                SessionManager.Instance.Username = user.Username;
                SessionManager.Instance.FullName = user.FullName;
                SessionManager.Instance.Role = user.Role;
                SessionManager.Instance.Email = user.Email;

                mainForm.Show();
                CloseCurrentWindow();
            }
            else
            {
                ErrorMessage = "* Incorrect username or password. Please try again.";
            }
        }

        private void CloseCurrentWindow()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is Views.LoginView loginView)
                {
                    loginView.Close();
                }
            }
        }
    }
}
