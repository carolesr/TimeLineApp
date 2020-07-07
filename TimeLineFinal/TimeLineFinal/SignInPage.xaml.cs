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
using Xamarin.Forms.Xaml;

namespace TimeLineFinal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private async void Registrar_Clicked(object sender, EventArgs e)
        {
            if (enLogin.Text == "" || enPass.Text == "" || enEmail.Text == "")
            {
                lbMessage.Text = "Dados de entrada inválidos.";
                lbMessage.IsVisible = true;
            }
            else
            {
                var new_player = await signup(enLogin.Text, enPass.Text, enEmail.Text);
                if (new_player == null)
                {
                    lbMessage.Text = "Erro.";
                    lbMessage.IsVisible = true;
                }
                else
                {
                    await this.Navigation.PushAsync(new InitialPage(new_player));
                }
            }

        }

        private async Task<Player> signup(string name, string password, string email)
        {
            try
            {
                WebClient wc = new WebClient();
                using (var stream = await wc.OpenReadTaskAsync($"http://timelinebe.azurewebsites.net/player/name={name},email={email},password={password}"))
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        string json = sr.ReadToEnd();
                        Player player = JsonConvert.DeserializeObject<Player>(json);
                        return player;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}