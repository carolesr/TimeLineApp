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
    public partial class GameOverPage : ContentPage
    {
        private int final_points;
        private Player user;

        public GameOverPage(int points, Player player)
        {
            InitializeComponent();
            user = player;
            final_points = points;
            lbPoints.Text = final_points.ToString();
        }

        private async void Inicio_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new InitialPage(user));
        }
    }
}