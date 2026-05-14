using System.Windows;
using VendingFranchisee.Desktop.Models;

namespace VendingFranchisee.Desktop
{
    public partial class App : Application
    {
        public static ApiUser? CurrentUser { get; set; }
    }
}