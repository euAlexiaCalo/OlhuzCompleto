using System;
using Microsoft.Maui.Controls; // Certifique-se de que este using está presente
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olhuz.ViewModels;

namespace Olhuz.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }

        // Esse método é executado automaticamente quando a página é navegada (aberta ou voltada para ela)
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            // Chama o comportamento padrão da navegação do método base para garantir que qualquer lógica de navegação existente seja executada
            base.OnNavigatedTo(args);

            // Ajusta o visual do cartão e aplica o estado "Normal" após a navegação
            VisualStateManager.GoToState(ReadImageCard, "Normal");
            VisualStateManager.GoToState(ReadingHistoryCard, "Normal");
            VisualStateManager.GoToState(SettingsCard, "Normal");
            VisualStateManager.GoToState(MyAccountCard, "Normal");
        }
    }
}