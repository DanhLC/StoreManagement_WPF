using Moq;
using StoreManagement.Models;
using StoreManagement.Services;
using StoreManagement.ViewModels;
using Xunit;

namespace StoreManagement.Tests.ViewModelTests
{
    public class UserViewModelTests
    {
        [Fact]
        public async Task LoginAsync_ShouldTriggerLoginSuccess_WhenCredentialsAreCorrect()
        {
            // Arrange
            var mockUserRepo = new Mock<IRepository<Users>>();
            var mockConfigRepo = new Mock<IConfigRepository>();
            var mockSessionManager = new Mock<ISessionManager>();

            // Dữ liệu giả cho repo
            var users = new List<Users>
            {
                new Users { Username = "testuser", Password = "password123" }
            };
            mockUserRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            var viewModel = new UserViewModel(mockUserRepo.Object, mockConfigRepo.Object, mockSessionManager.Object)
            {
                Username = "testuser",
                Password = "password123"
            };

            string triggeredAction = null;
            viewModel.RegisterUIAction(action => triggeredAction = action);

            // Act
            await viewModel.LoginAsync();

            // Assert
            Assert.Equal("LoginSuccess", triggeredAction);
            mockSessionManager.VerifySet(sm => sm.Username = "testuser", Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldSetErrorMessage_WhenCredentialsAreIncorrect()
        {
            // Arrange
            var mockUserRepo = new Mock<IRepository<Users>>();
            var mockConfigRepo = new Mock<IConfigRepository>();
            var mockSessionManager = new Mock<ISessionManager>();

            // Dữ liệu giả cho repo
            var users = new List<Users>
            {
                new Users { Username = "testuser", Password = "password123" }
            };
            mockUserRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            var viewModel = new UserViewModel(mockUserRepo.Object, mockConfigRepo.Object, mockSessionManager.Object)
            {
                Username = "wronguser",
                Password = "wrongpassword"
            };

            // Act
            await viewModel.LoginAsync();

            // Assert
            Assert.Equal("* Incorrect username or password. Please try again.", viewModel.ErrorMessage);
        }
    }
}
