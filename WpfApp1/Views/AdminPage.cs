using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VendingFranchisee.Desktop.Models;
using VendingFranchisee.Desktop.Services;
using System.Linq;

namespace VendingFranchisee.Desktop.Views
{
    public partial class AdminPage : Page
    {
        private readonly ApiService _apiService;
        private readonly NotificationService _notificationService;
        private List<ApiVendingMachine> _allMachines = new();

        public AdminPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            _notificationService = new NotificationService();
            LoadMachines();
        }

        private async void LoadMachines()
        {
            var machines = await _apiService.GetMachinesAsync();
            if (machines != null)
            {
                _allMachines = machines;
                dgMachines.ItemsSource = _allMachines;
            }
        }

        private void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            var filter = txtFilter.Text.ToLower();
            var filtered = _allMachines.Where(m =>
                m.Location.ToLower().Contains(filter) ||
                m.Model.ToLower().Contains(filter)).ToList();
            dgMachines.ItemsSource = filtered;
        }

        private void BtnAddMachine_Click(object sender, RoutedEventArgs e)
        {
            // Здесь можно открыть диалог добавления
            _notificationService.ShowNotification("Информация",
                "Функция добавления ТА в разработке",
                Services.NotificationType.Information);
        }

        private void MnuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgMachines.SelectedItem is ApiVendingMachine machine)
            {
                _notificationService.ShowNotification("Редактирование",
                    $"Редактирование ТА #{machine.MachineId}",
                    Services.NotificationType.Information);
            }
        }

        private void MnuUnbindModem_Click(object sender, RoutedEventArgs e)
        {
            if (dgMachines.SelectedItem is ApiVendingMachine machine)
            {
                var result = MessageBox.Show(
                    $"Отвязать модем от ТА #{machine.MachineId}?",
                    "Подтверждение", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    machine.ModemId = -1;
                    dgMachines.Items.Refresh();
                    _notificationService.ShowNotification("Успешно",
                        $"Модем отвязан от ТА #{machine.MachineId}",
                        Services.NotificationType.Information);
                }
            }
        }

        private async void MnuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgMachines.SelectedItem is ApiVendingMachine machine)
            {
                var result = MessageBox.Show(
                    $"Удалить ТА #{machine.MachineId}?",
                    "Подтверждение", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    var success = await _apiService.DeleteMachineAsync(machine.MachineId);
                    if (success)
                    {
                        _allMachines.Remove(machine);
                        dgMachines.ItemsSource = new List<ApiVendingMachine>(_allMachines);
                        _notificationService.ShowNotification("Удалено",
                            $"ТА #{machine.MachineId} удален",
                            Services.NotificationType.Information);
                    }
                }
            }
        }
    }
}