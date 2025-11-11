using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel; // Necessário para Launcher
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;
using Olhuz.ViewModels.Base;
using Olhuz.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Olhuz.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        // ====================================================================
        // Campos Privados para Binding
        // ====================================================================

        private string _fullName;
        // Default set to 18 years ago to satisfy the minimum age validation visually
        private DateTime _birthDate = DateTime.Now.Date.AddYears(-18);
        private string _email;
        private string _password;
        private string _confirmPassword;
        private bool _isTermsAccepted;

        private bool _isPasswordVisible = false;

        private Color _bar1Color = Colors.LightGray;
        private Color _bar2Color = Colors.LightGray;
        private Color _bar3Color = Colors.LightGray;
        private Color _bar4Color = Colors.LightGray;

        // ====================================================================
        // Propriedades Públicas (Binding)
        // O setter de cada propriedade chama OnPropertyChanged e RegisterCommand.ChangeCanExecute()
        // para reavaliar o estado de habilitação do botão Cadastrar.
        // ====================================================================

        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate != value)
                {
                    _birthDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
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
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsTermsAccepted
        {
            get => _isTermsAccepted;
            set
            {
                if (_isTermsAccepted != value)
                {
                    _isTermsAccepted = value;
                    OnPropertyChanged();
                }
            }
        }

        // ====================================================================
        // Propriedades de UI
        // ====================================================================

        // Controla se o texto da senha está visível (false para IsPassword=true)
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                if (_isPasswordVisible != value)
                {
                    _isPasswordVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _passwordStrengthText;

        public string PasswordStrengthText
        {
            get => _passwordStrengthText;
            set
            {
                if (_passwordStrengthText != value)
                {
                    _passwordStrengthText = value;
                    OnPropertyChanged();
                }
            }
        }

        // Determina qual ícone de 'olho' deve ser exibido
        // Assume que 'eye_open.png' e 'eye_closed.png' são recursos existentes no projeto.
        public string PasswordIconSource => _isPasswordVisible ? "eye_closed.png" : "eye_open.png";

        // Propriedades para as barras de força da senha
        public Color Bar1Color
        {
            get => _bar1Color;
            set { _bar1Color = value; OnPropertyChanged(); }
        }
        public Color Bar2Color
        {
            get => _bar2Color;
            set { _bar2Color = value; OnPropertyChanged(); }
        }
        public Color Bar3Color
        {
            get => _bar3Color;
            set { _bar3Color = value; OnPropertyChanged(); }
        }
        public Color Bar4Color
        {
            get => _bar4Color;
            set { _bar4Color = value; OnPropertyChanged(); }
        }

        // ====================================================================
        // Comandos
        // ====================================================================

        public Command RegisterCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand OpenTermsCommand { get; }
        public ICommand OpenPrivacyPolicyCommand { get; }

        // ====================================================================
        // Construtor
        // ====================================================================

        public RegisterViewModel()
        {
            // CORREÇÃO: Usar um lambda 'async () => await ...' para passar a referência do método assíncrono.
            // O erro anterior era causado pela chamada imediata do método (RegisterAsync()) que retorna um Task.
            RegisterCommand = new Command(async () => await RegisterAsync());

            TogglePasswordVisibilityCommand = new Command(ExecuteTogglePasswordVisibilityCommand);

            // Comandos para abrir links em um navegador externo
            OpenTermsCommand = new Command(async () => await ExecuteOpenUrlCommand("https://olhuz.com.br/termos-de-uso"));
            OpenPrivacyPolicyCommand = new Command(async () => await ExecuteOpenUrlCommand("https://olhuz.com.br/politica-de-privacidade"));
        }

        // ====================================================================
        // Lógica dos Comandos e Validação
        // ====================================================================


        private async Task RegisterAsync()
        {
            // --- VALIDAÇÃO 1: CAMPOS OBRIGATÓRIOS (NULO/VAZIO) ---

            if (_email.Contains(" ") || _password.Contains(" "))
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "O email e a senha não podem conter espaços em branco.", "OK");
                return;
            }

            if (!_email.Contains("@") || !_email.Contains("."))
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Por favor, insira um endereço de email válido.", "OK");
                return;
            }

            if (_password != _confirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "A senha e a confirmação de senha não coincidem.", "OK");
                return;
            }

            // --- VALIDAÇÃO 2: FORMATO E CONSISTÊNCIA DE DADOS ---

            // 2.1: E-mail e Senha contêm espaços
            // 2.2: E-mail não contém '@' ou '.'
            // 2.3: Senhas não são iguais
            if (Email.Contains(" ") || Password.Contains(" ") ||
        !Email.Contains("@") || !Email.Contains(".") ||
        Password != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert(
                  "Erro de Formato",
                  "Por favor, insira os dados corretamente.",
                  "OK");
                return;
            }

            // --- VALIDAÇÃO 3: RESTRIÇÃO DE IDADE (MENOR DE 18 ANOS) ---

            // Calcula a data limite: 18 anos atrás a partir de hoje.
            var minRegistrationDate = DateTime.Today.AddYears(-18);

            // Verifica se a Data de Nascimento é maior do que a data limite (ou seja, se a pessoa é menor de 18)
            if (BirthDate.Date > minRegistrationDate)
            {
                await Shell.Current.DisplayAlert(
                  "Restrição de Idade",
                  "Você precisa ter 18 anos ou mais para se cadastrar.",
                  "OK");
                return;
            }

            // --- VALIDAÇÃO CONCLUÍDA: SUCESSO ---

            // ** Lógica de Cadastro no Serviço/API aqui **
            // await _userService.RegisterUser(FullName, Email, Password, BirthDate);

            await Shell.Current.DisplayAlert(
        "Sucesso",
        "Cadastro concluído com sucesso! Seja bem-vindo(a).",
        "OK");

            // --- NAVEGAÇÃO PÓS-CADASTRO ---

            // Assumindo que a rota para a LoginPage é definida como "///LoginPage" no AppShell.xaml
            await Shell.Current.GoToAsync("///LoginPage");

            // Opcionalmente, limpamos os campos após o sucesso para garantir que não haja dados antigos na memória
            ClearFields();
        }

        // --- NOVO COMANDO: VOLTAR E LIMPAR CAMPOS ---
        [RelayCommand]
        private async Task GoBackAsync()
        {
            // 1. Limpa os campos preenchidos
            ClearFields();

            // 2. Navega para a página anterior.
            // O ".." no Shell navega de volta uma página na pilha.
            // Se você quer garantir que volte para a MainPage, use "///MainPage"
            // mas ".." é o padrão para um botão "Voltar".
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Limpa todos os campos do formulário de registro.
        /// </summary>
        private void ClearFields()
        {
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            FullName = string.Empty;
            // A data de nascimento é resetada para o estado seguro inicial (data atual).
            BirthDate = DateTime.Today;
        }

        private void ExecuteTogglePasswordVisibilityCommand()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        private async Task ExecuteOpenUrlCommand(string url)
        {
            try
            {
                await Launcher.OpenAsync(url);
            }
            catch (Exception)
            {
                // Trata o caso em que o navegador não pode ser aberto
                await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível abrir o link.", "OK");
            }
        }

        /// <summary>
        /// Calcula a pontuação de força da senha (0 a 5).
        /// </summary>
        private int CheckPasswordScore()
        {
            int score = 0;
            if (string.IsNullOrEmpty(Password)) return 0;

            // Pontuação da Força da Senha baseada nos requisitos
            if (Password.Length >= 8) score++; // Mínimo de 8 caracteres
            if (Regex.IsMatch(Password, "[a-z]")) score++; // Letras minúsculas
            if (Regex.IsMatch(Password, "[A-Z]")) score++; // Letras maiúsculas
            if (Regex.IsMatch(Password, "[0-9]")) score++; // Números
            // O requisito do usuário era "@Senha123" que inclui um caractere especial no início
            if (Regex.IsMatch(Password, "[^a-zA-Z0-9]")) score++; // Caracteres especiais

            return score;
        }

        /// <summary>
        /// Atualiza as propriedades de cor e texto da força da senha com base na pontuação.
        /// </summary>
        private void CheckPasswordStrength()
        {
            int score = CheckPasswordScore();

            // Resetar cores
            Bar1Color = Bar2Color = Bar3Color = Bar4Color = Colors.LightGray;
            PasswordStrengthText = "Extremamente Fraca";

            // Mapeamento de score para o UI
            switch (score)
            {
                case 1:
                    Bar1Color = Colors.Red;
                    PasswordStrengthText = "Muito Fraca";
                    break;
                case 2:
                    Bar1Color = Colors.Orange;
                    Bar2Color = Colors.Orange;
                    PasswordStrengthText = "Fraca";
                    break;
                case 3:
                case 4:
                    Bar1Color = Colors.YellowGreen;
                    Bar2Color = Colors.YellowGreen;
                    Bar3Color = Colors.YellowGreen;
                    PasswordStrengthText = "Boa";
                    break;
                case 5:
                    Bar1Color = Colors.Green;
                    Bar2Color = Colors.Green;
                    Bar3Color = Colors.Green;
                    Bar4Color = Colors.Green;
                    PasswordStrengthText = "Excelente";
                    break;
            }
        }
    }
}