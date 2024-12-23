﻿using FontAwesome.Sharp;
using StoreManagement.Core.Interfaces.Services;
using StoreManagement.UI.Views;
using System.Windows.Input;

namespace StoreManagement.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private readonly IViewFactory _viewFactory;
        private readonly ISessionManager _sessionManager;
        public ISessionManager Session => _sessionManager;

        #endregion

        #region Properties

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

        private string _caption;

        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        private IconChar _icon;

        public IconChar Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
        private bool IsDefaultLoading = false;

        #endregion

        #region Commands

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowOrderViewCommand { get; }
        public ICommand ShowCustomerViewCommand { get; }
        public ICommand ShowUserViewCommand { get; }

        #endregion

        public MainViewModel(
            ISessionManager sessionManager,
            IViewFactory viewFactory)
        {
            _sessionManager = sessionManager;
            _viewFactory = viewFactory;

            ShowDashboardCommand = new RelayCommand(_ => SetCurrentView("Dashboard"));
            ShowOrderViewCommand = new RelayCommand(_ => SetCurrentView("Order"));
            ShowCustomerViewCommand = new RelayCommand(_ => SetCurrentView("Customer"));
            ShowUserViewCommand = new RelayCommand(_ => SetCurrentView("User"));

            if (!IsDefaultLoading) SetCurrentView("Dashboard");
        }

        private void SetCurrentView(string viewName)
        {
            var descriptor = _viewFactory.CreateView(viewName);
            CurrentChildView = descriptor.ViewFactory.Invoke();
            Caption = descriptor.Caption;
            Icon = descriptor.Icon;
        }
    }
}
