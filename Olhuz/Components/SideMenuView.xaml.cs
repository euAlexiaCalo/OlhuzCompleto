// Olhuz.Components/SideMenuView.xaml.cs

using Microsoft.Maui.Controls;
using Olhuz.ViewModels;

namespace Olhuz.Components
{
    public partial class SideMenuView : ContentView
    {
        public SideMenuView()
        {
            InitializeComponent();

            this.Loaded += SideMenuView_Loaded;
            this.Unloaded += SideMenuView_Unloaded;
        }

        private void SideMenuView_Loaded(object sender, EventArgs e)
        {
            if (BindingContext is MenuViewModel viewModel)
            {
                // 🌟 CHAMA O NOVO MÉTODO PÚBLICO
                viewModel.SubscribeToShellEvents();
            }
        }

        private void SideMenuView_Unloaded(object sender, EventArgs e)
        {
            if (BindingContext is MenuViewModel viewModel)
            {
                // Chama Dispose() para remover a assinatura
                viewModel.Dispose();
            }
            // Limpa as assinaturas do próprio ContentView
            this.Loaded -= SideMenuView_Loaded;
            this.Unloaded -= SideMenuView_Unloaded;
        }
    }
}