using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StoreManagement.Models;
using StoreManagement.Services;

namespace StoreManagement.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        #region Fields

        private readonly IRepository<Users> _userRepository;
        private readonly IConfigRepository _configRepository;

        private bool _loginSuccessTriggered;
        private bool _focusPasswordTriggered;
        public Action<string> UIAction { get; set; }

        #endregion

        #region Constructors
        public UserViewModel(
            IRepository<Users> userRepository,
            IConfigRepository configRepository)
        {
            _userRepository = userRepository;
            _configRepository = configRepository;
            IsPasswordVisible = false;

            _ = InitializeAsync();
        }

        #endregion

        #region Initialization

        private async Task InitializeAsync()
        {
            await LoadInformation();
            await ApplyQuickLoginAsync();
        }

        #endregion

        #region Properties

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

        [ObservableProperty]
        private string loginTitle;

        [ObservableProperty]
        private string loginLogo;

        public string SecurePassword { get; set; }

        #endregion

        #region Commands

        public IRelayCommand LoginCommand => new AsyncRelayCommand(LoginAsync);

        #endregion

        #region Task

        private async Task LoginAsync()
        {
            if (IsPasswordVisible && Password != PlainPassword) Password = PlainPassword;

            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (user != null)
            {
                SetUserSessionValue(user);
                _loginSuccessTriggered = true;
                UIAction?.Invoke("LoginSuccess");
            }
            else
            {
                ErrorMessage = "* Incorrect username or password. Please try again.";
            }
        }

        private async Task LoadInformation()
        {
            var lastUsername = await _configRepository.GetByKeyAsync("LastLoggedInUsername");
            var loginTitle = await _configRepository.GetByKeyAsync("LoginTitle");
            var loginLogo = await _configRepository.GetByKeyAsync("LoginLogo");

            Username = lastUsername?.Value ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(Username))
            {
                _focusPasswordTriggered = true;
                UIAction?.Invoke("FocusPasswordBox");
            }

            LoginTitle = loginTitle?.Value?? "Demo title";
            LoginLogo = !string.IsNullOrEmpty(loginLogo?.Value) ? string.Format("/Images/{0}", loginLogo.Value) : "/Images/default-login-logo.png";
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
                    UIAction?.Invoke("LoginSuccess");
                }
            }
        }

        #endregion

        #region Set User Session Value
        private void SetUserSessionValue(Users user)
        {
            SessionManager.Instance.UserId = user.Id;
            SessionManager.Instance.Username = user.Username;
            SessionManager.Instance.FullName = user.FullName;
            SessionManager.Instance.Role = user.Role;
            SessionManager.Instance.Email = user.Email;
        }

        #endregion

        public void RegisterUIAction(Action<string> uiAction)
        {
            UIAction = uiAction;

            if (_loginSuccessTriggered)
            {
                UIAction.Invoke("LoginSuccess");
            }
            else if (_focusPasswordTriggered)
            {
                UIAction.Invoke("FocusPasswordBox");
            }
        }
    }
}
