using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoreManagement.Models;
using StoreManagement.Services;
using System.Windows;

namespace StoreManagement.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        private readonly IRepository<Users> _userRepository;
        private readonly IConfigRepository _configRepository;
        private bool _loginSuccessTriggered;
        private event Action _onLoginSuccess;

        public event Action OnLoginSuccess
        {
            add
            {
                if (_loginSuccessTriggered)
                {
                    value.Invoke();
                }
                else
                {
                    _onLoginSuccess += value;
                }
            }
            remove => _onLoginSuccess -= value;
        }

        public UserViewModel(
            IRepository<Users> userRepository,
            IConfigRepository configRepository)
        {
            _userRepository = userRepository;
            _configRepository = configRepository;
            IsPasswordVisible = false;

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadLastLoginAsync();
            await ApplyQuickLoginAsync();
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

        public IRelayCommand LoginCommand => new AsyncRelayCommand(LoginAsync);

        private async Task LoginAsync()
        {
            if (IsPasswordVisible && Password != PlainPassword) Password = PlainPassword;

            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (user != null)
            {
                SetUserSessionValue(user);
                _loginSuccessTriggered = true;
                _onLoginSuccess?.Invoke();
            }
            else
            {
                ErrorMessage = "* Incorrect username or password. Please try again.";
            }
        }

        private async Task LoadLastLoginAsync()
        {
            var lastUsername = await _configRepository.GetByKeyAsync("LastLoggedInUsername");

            Username = lastUsername?.Value ?? string.Empty;
        }

        public async Task ApplyQuickLoginAsync()
        {
            var applyQuickLogin = await _configRepository.GetByKeyAsync("ApplyQuickLogin");

            if (applyQuickLogin != null && bool.TryParse(applyQuickLogin.Value, out bool isQuickLoginEnabled) && isQuickLoginEnabled)
            {
                var users = await _userRepository.GetAllAsync();
                var user = users.FirstOrDefault(u => u.Username == Username);

                if (user != null)
                {
                    SetUserSessionValue(user);
                    _loginSuccessTriggered = true;
                    _onLoginSuccess?.Invoke();
                }
            }
        }

        private void SetUserSessionValue(Users user)
        {
            SessionManager.Instance.UserId = user.Id;
            SessionManager.Instance.Username = user.Username;
            SessionManager.Instance.FullName = user.FullName;
            SessionManager.Instance.Role = user.Role;
            SessionManager.Instance.Email = user.Email;
        }
    }
}
