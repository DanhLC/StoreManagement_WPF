﻿using Moq;
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
                new Users { Username = "testuser", Password = "password123" }
            };
            mockUserRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            var viewModel = new LoginViewModel(mockUserRepo.Object, mockConfigRepo.Object, mockSessionManager.Object)
            {
                Username = "testuser",
                Password = "password123"
            };

            string triggeredAction = null;
            viewModel.RegisterUIAction(action => triggeredAction = action);

            await viewModel.LoginAsync();

            Assert.Equal("LoginSuccess", triggeredAction);
            mockSessionManager.VerifySet(sm => sm.Username = "testuser", Times.Once);
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
    }
}
