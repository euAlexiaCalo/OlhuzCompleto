using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Olhuz.Views;

namespace Olhuz.ViewModels
{
    public class HomeViewModel : BindableObject
    {

        public ICommand ReadImagePageCommand { get; }

        public ICommand ReadingHistoryPageCommand { get; }

        public ICommand SettingsPageCommand { get; }

        public ICommand MyAccountPageCommand { get; }

        public HomeViewModel()
        {
            ReadImagePageCommand = new Command(async () => await GoReadImagePageAsync());
            ReadingHistoryPageCommand = new Command(async () => await GoReadingHistoryPageAsync());
            SettingsPageCommand = new Command(async () => await GoSettingsPageAsync());
            MyAccountPageCommand = new Command(async () => await GoMyAccountPageAsync());
        }

        private Task GoReadImagePageAsync()
        {
            return Shell.Current.GoToAsync($"//{nameof(ReadImagePage)}");
        }

        private Task GoReadingHistoryPageAsync()
        {
            return Shell.Current.GoToAsync($"//{nameof(ReadingHistoryPage)}");
        }
        private Task GoSettingsPageAsync()
        {
            return Shell.Current.GoToAsync($"//{nameof(SettingsPage)}");
        }
        private Task GoMyAccountPageAsync()
        {
            return Shell.Current.GoToAsync($"//{nameof(MyAccountPage)}");
        }
    }
}
