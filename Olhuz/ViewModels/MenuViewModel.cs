using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Olhuz.Models;
using Olhuz.Views;

namespace Olhuz.ViewModels
{
    public class MenuViewModel : INotifyPropertyChanged, IDisposable // 👈 Adicione IDisposable
    {
        // ... (Implementação de INotifyPropertyChanged: PropertyChanged, SetProperty, OnPropertyChanged) ...
        public event PropertyChangedEventHandler PropertyChanged;
        // ... (Implementação dos métodos auxiliares SetProperty e OnPropertyChanged) ...

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<MenuItemModel> Items { get; set; }

        private MenuItemModel _currentSelectedItem;
        public MenuItemModel CurrentSelectedItem
        {
            get => _currentSelectedItem;
            set => SetProperty(ref _currentSelectedItem, value);
        }

        public ICommand NavigateCommand { get; }

        public MenuViewModel()
        {
            Items = new ObservableCollection<MenuItemModel>
            {
                new MenuItemModel { Title="Menu Principal", Icon="home.png", TargetPage = typeof(HomePage)},
                new MenuItemModel { Title="Ler Imagens", Icon="home.png", TargetPage = typeof(ReadImagePage)},
                new MenuItemModel { Title="Leituras Recentes", Icon="home.png", TargetPage = typeof(ReadingHistoryPage)},
                new MenuItemModel { Title="Configurações", Icon="settings.png", TargetPage = typeof(SettingsPage)},
                new MenuItemModel { Title="Minha Conta", Icon="settings.png", TargetPage = typeof(MyAccountPage)},
            };

            NavigateCommand = new Command<MenuItemModel>(async (item) => await OnNavigateCommandExecuted(item));

            // 🛑 NOTA: Removida a assinatura do construtor, que será feita no Code-Behind
        }

        // 🌟 NOVO MÉTODO PÚBLICO PARA ASSINAR EVENTOS 🌟
        public void SubscribeToShellEvents()
        {
            // O Shell.Current deve estar disponível aqui, pois este método é chamado no Loaded da View
            if (Shell.Current != null)
            {
                // Remove a assinatura antiga antes de adicionar uma nova (para evitar duplicidade)
                Shell.Current.Navigated -= Shell_Navigated;
                Shell.Current.Navigated += Shell_Navigated;
            }

            // Dispara a lógica de seleção inicial imediatamente
            Shell_Navigated(this, null);
        }

        private void Shell_Navigated(object sender, ShellNavigatedEventArgs e)
        {
            var currentRoute = Shell.Current?.CurrentState?.Location?.OriginalString;

            if (string.IsNullOrWhiteSpace(currentRoute)) return;

            var currentRouteName = currentRoute.Split('/').LastOrDefault();

            if (string.IsNullOrWhiteSpace(currentRouteName)) return;

            var newSelectedItem = Items.FirstOrDefault(item =>
                item.TargetPage != null && item.TargetPage.Name == currentRouteName);

            if (newSelectedItem != null && newSelectedItem != CurrentSelectedItem)
            {
                if (CurrentSelectedItem != null)
                {
                    CurrentSelectedItem.IsSelected = false;
                }

                newSelectedItem.IsSelected = true;
                CurrentSelectedItem = newSelectedItem;
            }
        }

        private async Task OnNavigateCommandExecuted(MenuItemModel item)
        {
            if (item == null || item == CurrentSelectedItem) return;

            if (item.TargetPage != null)
            {
                try
                {
                    await Shell.Current.GoToAsync($"//{item.TargetPage.Name}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro de navegação: {ex.Message}");
                }
            }
        }

        // 🌟 IMPLEMENTAÇÃO DO DISPOSE
        public void Dispose()
        {
            if (Shell.Current != null)
            {
                Shell.Current.Navigated -= Shell_Navigated;
            }
        }
    }
}