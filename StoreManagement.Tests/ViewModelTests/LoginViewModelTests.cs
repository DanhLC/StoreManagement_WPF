using Moq;
using StoreManagement.Core;
using StoreManagement.Core.Interfaces.Services;
using StoreManagement.UI.ViewModels;
using Xunit;

namespace StoreManagement.UI.Tests.ViewModelTests
{
    public class LoginViewModelTests
    {
        [Fact]
        public async Task LoginAsync_ShouldTriggerLoginSuccess_WhenCredentialsAreCorrect()
        {
            var mockUserRepo = new Mock<IRepository<Users>>();
            var mockConfigRepo = new Mock<IConfigRepository>();
            var mockSessionManager = new Mock<ISessionManager>();

            var users = new List<Users>
            {
                new Users
                {
                    Username = "testuser",
                    Password = "password123",
                    Id = 1,
                    FullName = "Test User",
                    Role = "Admin",
                    Email = "testuser@example.com"
                }
            };
            mockUserRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            var viewModel = new LoginViewModel(mockUserRepo.Object, mockConfigRepo.Object, mockSessionManager.Object)
            {
                Username = "testuser",
                Password = "password123"
            };

            var triggeredAction = (string?)null;
            viewModel.RegisterUIAction(action => triggeredAction = action);

            await viewModel.LoginAsync();

            mockSessionManager.VerifySet(sm => sm.UserId = 1, Times.Once);
            mockSessionManager.VerifySet(sm => sm.FullName = "Test User", Times.Once);
            mockSessionManager.VerifySet(sm => sm.Role = "Admin", Times.Once);
            mockSessionManager.VerifySet(sm => sm.Email = "testuser@example.com", Times.Once);
            Assert.Equal("LoginSuccess", triggeredAction);
        }

        [Fact]
        public async Task LoginAsync_ShouldSetErrorMessage_WhenCredentialsAreIncorrect()
        {
            var mockUserRepo = new Mock<IRepository<Users>>();
            var mockConfigRepo = new Mock<IConfigRepository>();
            var mockSessionManager = new Mock<ISessionManager>();

            var users = new List<Users>
            {
                new Users { Username = "testuser", Password = "password123" }
            };

            mockUserRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            var viewModel = new LoginViewModel(mockUserRepo.Object, mockConfigRepo.Object, mockSessionManager.Object)
            {
                Username = "wronguser",
                Password = "wrongpassword"
            };

            await viewModel.LoginAsync();

            Assert.Equal("* Incorrect username or password. Please try again.", viewModel.ErrorMessage);
        }

        [Fact]
        public async Task LoginAsync_ShouldSetErrorMessage_WhenUserNotFound()
        {
            var mockUserRepo = new Mock<IRepository<Users>>();
            var mockConfigRepo = new Mock<IConfigRepository>();
            var mockSessionManager = new Mock<ISessionManager>();

            var users = new List<Users>(); 
            mockUserRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            var viewModel = new LoginViewModel(mockUserRepo.Object, mockConfigRepo.Object, mockSessionManager.Object)
            {
                Username = "testuser",
                Password = "password123"
            };

            await viewModel.LoginAsync();

            Assert.Equal("* Incorrect username or password. Please try again.", viewModel.ErrorMessage);
        }

        [Fact]
        public async Task ApplyQuickLoginAsync_ShouldLoginUserAutomatically_WhenQuickLoginIsEnabled()
        {
            var mockUserRepo = new Mock<IRepository<Users>>();
            var mockConfigRepo = new Mock<IConfigRepository>();
            var mockSessionManager = new Mock<ISessionManager>();

            var users = new List<Users>
            {
                new Users { Username = "testuser", Password = "password123" }
            };
            mockUserRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);
            mockConfigRepo.Setup(repo => repo.GetByKeyAsync("ApplyQuickLogin")).ReturnsAsync(new Config { Value = "true" });

            var viewModel = new LoginViewModel(mockUserRepo.Object, mockConfigRepo.Object, mockSessionManager.Object)
            {
                Username = "testuser"
            };

            var triggeredAction = (string?)null;
            viewModel.RegisterUIAction(action => triggeredAction = action);

            await viewModel.ApplyQuickLoginAsync();

            mockSessionManager.VerifySet(sm => sm.Username = "testuser", Times.Once);
            Assert.Equal("LoginSuccess", triggeredAction);
        }
    }
}
