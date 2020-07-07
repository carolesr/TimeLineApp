using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeLineFinal.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeLineFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialPage : ContentPage
    {
        public Player user;

        public InitialPage(Player player = null)
        {
            InitializeComponent();
            user = player;
            lbUser.Text = "Bem vind@ " + user.Username + "!";
        }

        private async void Jogar_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new GamePage(user));
        }

        private async void Estatisticas_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new StatisticsPage(user));
        }
    }
}