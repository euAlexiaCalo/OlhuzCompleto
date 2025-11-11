using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls; // Para usar a classe Color
using Olhuz.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Olhuz.Views;

namespace Olhuz.ViewModels
{
    // Adicionamos 'partial' para permitir o uso do [RelayCommand] no método GoToRegisterPageCommand
    public partial class LoginViewModel : BaseViewModel
    {
        // ====================================================================
        // Campos Privados
        // ====================================================================

        // No MVVM, use backing fields privados para as propriedades bindáveis
        private string _email;
        private string _password;

        // Adicionamos uma propriedade que representa o estado de 'ocultar a senha' no Entry.
        // O Entry espera true (ocultar) ou false (mostrar).
        private bool _isPasswordMode = true; // Senha deve começar oculta (true)
        private string _passwordIconSource = "olhofechado.png";
        private bool _isBusy;

        // ====================================================================
        // Propriedades Bindáveis Públicas
        // ====================================================================

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                    // Opcional: Atualizar o estado do LoginCommand (NotifyCanExecuteChanged)
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                    // Opcional: Atualizar o estado do LoginCommand (NotifyCanExecuteChanged)
                }
            }
        }

        /// <summary>
        /// PROPRIEDADE CORRIGIDA. Indica se o Entry deve ocultar o texto da senha.
        /// TRUE = Ocultar (modo senha ativo). FALSE = Mostrar (texto normal).
        /// Esta é a propriedade que você deve bindar ao Entry.IsPassword no XAML.
        /// </summary>
        public bool IsPasswordMode
        {
            get => _isPasswordMode;
            set
            {
                if (_isPasswordMode != value)
                {
                    _isPasswordMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PasswordIconSource
        {
            get => _passwordIconSource;
            set
            {
                if (_passwordIconSource != value)
                {
                    _passwordIconSource = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                    // Certifica-se de que a visibilidade do botão de login seja atualizada
                    (LoginCommand as Command)?.ChangeCanExecute();
                }
            }
        }

        // ====================================================================
        // Comandos
        // ====================================================================

        // Definido como Command ou RelayCommand
        public ICommand LoginCommand { get; }

        // Mantenho o RelayCommand para esta lógica de UI
        [RelayCommand]
        private void TogglePasswordVisibility()
        {
            // 1. Inverte o valor de IsPasswordMode
            // O Setter chama OnPropertyChanged, que notifica o Entry no XAML.
            IsPasswordMode = !IsPasswordMode;

            // 2. Atualiza o ícone
            PasswordIconSource = IsPasswordMode ? "olhofechado.png" : "olhoaberto.png";
        }

        [RelayCommand]
        private async Task GoToRegisterPageAsync()
        {
            // Navega para a página de registro
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        // ====================================================================
        // Construtor
        // ====================================================================

        public LoginViewModel()
        {
            // O LoginCommand pode ser um Command simples, mas deve verificar CanExecute
            LoginCommand = new Command(async () => await ExecuteLoginAsync(), CanExecuteLogin);
        }

        // ====================================================================
        // Lógica dos Comandos
        // ====================================================================

        /// <summary>
        /// Lógica de validação simples para o botão de login.
        /// </summary>
        /// <returns>True se o email e a senha não estiverem vazios.</returns>
        private bool CanExecuteLogin()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && !IsBusy;
        }

        private async Task ExecuteLoginAsync()
        {
            IsBusy = true; // Inicia o estado de ocupado

            // Garante que a comparação de email é case-insensitive e a da senha é case-sensitive
            string email = Email?.ToLower() ?? string.Empty;
            string senha = Password ?? string.Empty;

            bool loginValido = false;

            // --- Lógica de Validação Mock (Substituir por chamada de API real) ---

            // Simula um delay de rede
            await Task.Delay(1500);

            if (email == "teste@olhuz.com" && senha == "@Olhuz1234")
            {
                loginValido = true;
            }
            else if (email == "teste@senac.com" && senha == "@Senac1234")
            {
                loginValido = true;
            }

            // --- Fim da Lógica de Validação Mock ---

            if (loginValido)
            {
                await Shell.Current.DisplayAlert("Sucesso", "Login efetuado com sucesso!", "OK");
                // Exemplo de navegação para a página principal
                // await Shell.Current.GoToAsync("//MainPage"); 
            }
            else
            {
                await Shell.Current.DisplayAlert("Erro", "Email ou senha incorretos.", "OK");
            }

            IsBusy = false; // Finaliza o estado de ocupado
        }
    }
}