using System.Windows;
using System.Windows.Controls;
using VendingFranchisee.Desktop.Models;

namespace VendingFranchisee.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(ApiUser user)
        {
            InitializeComponent();
            tbUserName.Text = user.FullName;
            tbUserRole.Text = user.Role;
            MainFrame.Navigate(new HomePage());
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e) =>
            MainFrame.Navigate(new HomePage());

        private void BtnMonitor_Click(object sender, RoutedEventArgs e) =>
            MainFrame.Navigate(new MonitorPage());

        private void BtnAdmin_Click(object sender, RoutedEventArgs e) =>
            MainFrame.Navigate(new AdminPage());
    }
}