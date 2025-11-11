using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Olhuz.Views;
using Olhuz.Models;
using Olhuz.ViewModels.Base;

namespace Olhuz.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }

        public MainViewModel()
        {
            RegisterCommand = new Command(async () => await GoRegisterPageAsync());
            LoginCommand = new Command(async () => await GoLoginPageAsync());
        }

        private Task GoRegisterPageAsync()
        {
            return Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }

        private Task GoLoginPageAsync()
        {
            return Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }
    }
}
