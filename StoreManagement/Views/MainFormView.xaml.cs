using StoreManagement.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace StoreManagement.Views
{
    /// <summary>
    /// Interaction logic for MainFormView.xaml
    /// </summary>
    public partial class MainFormView : Window
    {
        public MainFormView(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
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

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }
        private void pnControlBar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        { 
            DragMove();
        }
    }
}
