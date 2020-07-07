using Newtonsoft.Json;
using System;
using System.Collections;
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
    public partial class StatisticsPage : ContentPage
    {
        private Player user;

        public StatisticsPage(Player player)
        {
            InitializeComponent();
            user = player;
            getData();
        }

        private async Task<Statistics> userStatistics(int pid)
        {
            try
            {
                WebClient wc = new WebClient();
                using (var stream = await wc.OpenReadTaskAsync($"http://timelinebe.azurewebsites.net/match/player={pid}"))
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        string json = sr.ReadToEnd();
                        Statistics data = JsonConvert.DeserializeObject<Statistics>(json);
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async void getData()
        {
            var data = await userStatistics(user.ID);
            lbRate.Text = "Taxa de acerto: " + data.SuccessRate + ".";
            lbHits.Text = "Pontuação máxima: " + data.ConsecutiveHits + ".";
        }

        private async void Voltar_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PopAsync();
        }
    }
}