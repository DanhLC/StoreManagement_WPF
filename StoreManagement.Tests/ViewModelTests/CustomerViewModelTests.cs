using Moq;
using StoreManagement.Core;
using StoreManagement.Core.Interfaces.Builders;
using StoreManagement.Core.Interfaces.Services;
using StoreManagement.UI.ViewModels;
using System.Linq.Expressions;
using Xunit;

namespace StoreManagement.Tests.ViewModelTests
{
    public class CustomerViewModelTests
    {
        private readonly Mock<IRepository<Customers>> _customerRepositoryMock;
        private readonly Mock<IViewFactory> _viewFactoryMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<ICustomerBuilder> _customerBuilderMock;
        private readonly CustomerViewModel _viewModel;

        public CustomerViewModelTests()
        {
            _customerRepositoryMock = new Mock<IRepository<Customers>>();
            _viewFactoryMock = new Mock<IViewFactory>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _customerBuilderMock = new Mock<ICustomerBuilder>();
        }

        [Fact]
        public async Task LoadPageAsync_ShouldLoadCustomersCorrectly_WhenListCustomerFound()
        {
            var customersData = new List<Customers>
            {
                new Customers { Id = 1, FullName = "John Doe", Address = "123 Street", Phone = "1234567890", DebtAmount = 1000 },
                new Customers { Id = 2, FullName = "Jane Smith", Address = "456 Avenue", Phone = "0987654321", DebtAmount = 2000 }
            };

            _customerRepositoryMock
                .Setup(repo => repo.GetPagedAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Expression<Func<Customers, bool>>>(),
                    It.IsAny<Expression<Func<Customers, object>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync((customersData, 2));

            _customerBuilderMock
                .Setup(b => b.SetCurrentCustomer(It.IsAny<Customers>()))
                .Returns(_customerBuilderMock.Object);
            _customerBuilderMock
                .Setup(b => b.SetIdentityNumber(It.IsAny<int>()))
                .Returns(_customerBuilderMock.Object);
            _customerBuilderMock
                .Setup(b => b.SetBgColor(It.IsAny<string[]>(), It.IsAny<Random>()))
                .Returns(_customerBuilderMock.Object);
            _customerBuilderMock
                .Setup(b => b.SetCharacter())
                .Returns(_customerBuilderMock.Object);
            _customerBuilderMock
                .Setup(b => b.SetDebtAmountString(It.IsAny<decimal>()))
                .Returns(_customerBuilderMock.Object);
            _customerBuilderMock
                .Setup(b => b.AddToList())
                .Returns(_customerBuilderMock.Object);
            _customerBuilderMock
                .Setup(b => b.BuildList())
                .Returns(customersData);

            var viewModel = new CustomerViewModel(
                _customerRepositoryMock.Object,
                _viewFactoryMock.Object,
                _serviceProviderMock.Object,
                _customerBuilderMock.Object);

            await viewModel.LoadPageAsync(1);

            Assert.Equal(2, viewModel.Customers.Count);
            Assert.Equal(2, viewModel.TotalResults);
            Assert.Equal(1, viewModel.CurrentPage);
            _customerRepositoryMock.Verify(repo => repo.GetPagedAsync(
                1, viewModel.PageSize,
                It.IsAny<Expression<Func<Customers, bool>>>(),
                It.IsAny<Expression<Func<Customers, object>>>(),
                true), Times.Once);
            _customerBuilderMock.Verify(b => b.AddToList(), Times.Exactly(2));
        }

        [Fact]
        public async Task LoadPageAsync_ShouldLoadCustomersCorrectly_WhenListCustomerNotFound()
        {
            _customerRepositoryMock
                .Setup(repo => repo.GetPagedAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Expression<Func<Customers, bool>>>(),
                    It.IsAny<Expression<Func<Customers, object>>>(),
                    It.IsAny<bool>()))
                .ReturnsAsync((new List<Customers>(), 0));

            _customerBuilderMock
                .Setup(b => b.SetCurrentCustomer(It.IsAny<Customers>()))
                .Returns(_customerBuilderMock.Object);
            _customerBuilderMock
                .Setup(b => b.AddToList())
                .Returns(_customerBuilderMock.Object);
            _customerBuilderMock
                .Setup(b => b.BuildList())
                .Returns(new List<Customers>());

            var viewModel = new CustomerViewModel(
                _customerRepositoryMock.Object,
                _viewFactoryMock.Object,
                _serviceProviderMock.Object,
                _customerBuilderMock.Object);

            await viewModel.LoadPageAsync(1);

            Assert.NotNull(viewModel.Customers);
            Assert.Empty(viewModel.Customers);
            Assert.Equal(0, viewModel.TotalResults);
            Assert.Equal(0, viewModel.CurrentPage);

            _customerRepositoryMock.Verify(repo => repo.GetPagedAsync(
                1, viewModel.PageSize,
                It.IsAny<Expression<Func<Customers, bool>>>(),
                It.IsAny<Expression<Func<Customers, object>>>(),
                true), Times.Once);

            _customerBuilderMock.Verify(b => b.AddToList(), Times.Never);
        }

    }
}
