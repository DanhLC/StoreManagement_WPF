using CommunityToolkit.Mvvm.Input;
using StoreManagement.Services;
using StoreManagement.Views;
using System.Windows.Input;

namespace StoreManagement.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        private readonly IViewFactory _viewFactory;
        private readonly ISessionManager _sessionManager;

        public ISessionManager Session => _sessionManager;
        private object _currentChildView;

        public object CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        #endregion

        #region Commands

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowOrderViewCommand { get; }
        public ICommand ShowCustomerViewCommand { get; }

        #endregion

        public MainViewModel(
            ISessionManager sessionManager,
            IViewFactory viewFactory)
        {
            _sessionManager = sessionManager;
            _viewFactory = viewFactory;

            ShowDashboardCommand = new RelayCommand(_ => CurrentChildView = _viewFactory.CreateView("Dashboard"));
            ShowOrderViewCommand = new RelayCommand(_ => CurrentChildView = _viewFactory.CreateView("Order"));
            ShowCustomerViewCommand = new RelayCommand(_ => CurrentChildView = _viewFactory.CreateView("Customer"));

            CurrentChildView = _viewFactory.CreateView("Dashboard");
        }
    }
}
