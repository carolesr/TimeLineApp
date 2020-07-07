using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TimeLineFinal.Models;
using Xamarin.Forms;

namespace TimeLineFinal
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Entrar_Clicked(object sender, EventArgs e)
        {
            var player = await login(enLogin.Text, enPass.Text);
            if (player == null)
            {
                lbMessage.Text = "Usuário ou senha incorreta.";
                lbMessage.IsVisible = true;
            }
            else
            {
                await this.Navigation.PushAsync(new InitialPage(player));
            }
        }

        private async Task<Player> login(string name, string password)
        {
            try
            {
                WebClient wc = new WebClient();
                using (var stream = await wc.OpenReadTaskAsync($"http://timelinebe.azurewebsites.net/player/name={name},password={password}"))
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        string json = sr.ReadToEnd();
                        Player lista_players = JsonConvert.DeserializeObject<Player>(json);
                        return lista_players;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Game> game()
        {
            try
            {
                WebClient wc = new WebClient();
                using (var stream = await wc.OpenReadTaskAsync($"http://timelinebe.azurewebsites.net/game"))
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        string json = sr.ReadToEnd();
                        Game lista_players = JsonConvert.DeserializeObject<Game>(json);
                        return lista_players;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void Novo_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new SignInPage());
        }

    }
}
