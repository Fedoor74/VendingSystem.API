using System;
using System.IO;

namespace VendingFranchisee.Desktop.Services
{
    public enum NotificationType
    {
        Critical,
        Warning,
        Information
    }

    public class NotificationService
    {
        public event Action<string, string, NotificationType>? OnNotification;
        private readonly string _logFilePath;

        public NotificationService()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "notifications.log");
        }

        public void ShowNotification(string title, string message, NotificationType type)
        {
            LogNotification(title, message, type);
            OnNotification?.Invoke(title, message, type);
        }

        private void LogNotification(string title, string message, NotificationType type)
        {
            try
            {
                var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{type}] {title}: {message}{Environment.NewLine}";
                File.AppendAllText(_logFilePath, logEntry);
            }
            catch { }
        }

        public void SimulateNotifications()
        {
            var random = new Random();
            var messages = new[]
            {
                ("Критическая ошибка", "Нет сдачи в ТА №5", NotificationType.Critical),
                ("Предупреждение", "Заканчивается Кофе в ТА №2", NotificationType.Warning),
                ("Информация", "Успешная инкассация ТА №3", NotificationType.Information),
                ("Критическая ошибка", "Замятие товара в ТА №7", NotificationType.Critical),
                ("Предупреждение", "Низкий заряд батареи ТА №1", NotificationType.Warning)
            };

            var msg = messages[random.Next(messages.Length)];
            ShowNotification(msg.Item1, msg.Item2, msg.Item3);
        }
    }
}