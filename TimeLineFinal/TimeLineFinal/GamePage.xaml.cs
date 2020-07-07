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
    public partial class GamePage : ContentPage
    {
        private Player user;
        private List<Measure> botoes;
        private List<double> ordem;
        private Game jogo;
        private List<Measure> lista_itens;
        private int round;
        public int points;

        public GamePage(Player player)
        {
            InitializeComponent();
            user = player;
            getGame();
            botoes = new List<Measure>();
            ordem = new List<double>();
            points = 0;
            round = 1;
        }

        private async void getGame()
        {
            jogo = await game();
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
                        Game game = JsonConvert.DeserializeObject<Game>(json);
                        this.lista_itens = await measure(game.Measure1, game.Measure2, game.Measure3);
                        initializeButtons();
                        return game;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Match> makeMatch(int pid, int gid, int score)
        {
            try
            {
                WebClient wc = new WebClient();
                using (var stream = await wc.OpenReadTaskAsync($"http://timelinebe.azurewebsites.net/match/game={gid},player={pid},score={score}"))
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        string json = sr.ReadToEnd();
                        Match match = JsonConvert.DeserializeObject<Match>(json);
                        return match;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void getMeasures()
        {
            lista_itens = await measure(jogo.Measure1, jogo.Measure2, jogo.Measure3);
        }

        private async Task<List<Measure>> measure(int id1, int id2, int id3)
        {
            try
            {
                WebClient wc = new WebClient();
                using (var stream = await wc.OpenReadTaskAsync($"http://timelinebe.azurewebsites.net/game/m1={id1},m2={id2},m3={id3}"))
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        string json = sr.ReadToEnd();
                        List<Measure> lista_item = JsonConvert.DeserializeObject<List<Measure>>(json);
                        return lista_item;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void initializeButtons()
        {
            lbRound.Text = "Rodada " + round;

            System.Random rand = new System.Random(DateTime.Now.Millisecond);
            var randlist = lista_itens.OrderBy(x => rand.Next(0, 100)); //embaralhando os botões

            btButton1.Text = lista_itens[0].Description;
            btButton2.Text = lista_itens[1].Description;
            btButton3.Text = lista_itens[2].Description;

            btButton1.BackgroundColor = Color.LightGray;
            btButton2.BackgroundColor = Color.LightGray;
            btButton3.BackgroundColor = Color.LightGray;
            Continuar.IsVisible = false;

            ordem.Clear();
            var sorted = lista_itens.OrderBy(x => x.ValueMeasuer); //ordena acontecimentos pelo tempo
            var aux = sorted.ToList();
            ordem.Add(aux[0].ValueMeasuer);
            ordem.Add(aux[1].ValueMeasuer);
            ordem.Add(aux[2].ValueMeasuer);

            botoes.Clear();

        }


        private void Button1_Clicked(object sender, EventArgs e)
        {
            btButton1.BackgroundColor = Color.DarkGray;
            botoes.Add(lista_itens[0]);

            if (botoes.Count() == 3)
                ApertouTodos();

        }
        private void Button2_Clicked(object sender, EventArgs e)
        {
            btButton2.BackgroundColor = Color.DarkGray;
            botoes.Add(lista_itens[1]);

            if (botoes.Count() == 3)
                ApertouTodos();

        }
        private void Button3_Clicked(object sender, EventArgs e)
        {
            btButton3.BackgroundColor = Color.DarkGray;
            botoes.Add(lista_itens[2]);

            if (botoes.Count() == 3)
                ApertouTodos();
        }

        private bool ApertouTodos()
        {
            btButton1.Text = lista_itens[0].Awnser;
            btButton2.Text = lista_itens[1].Awnser;
            btButton3.Text = lista_itens[2].Awnser;

            bool acertou = true;
            for (int n = 0; n < 3; n++)
            {
                if (botoes[n].ValueMeasuer != ordem[n])
                {
                    acertou = false;
                    break;
                }
            }

            Continuar.IsVisible = true;

            if (acertou)
            {
                btButton1.BackgroundColor = Color.Green;
                btButton2.BackgroundColor = Color.Green;
                btButton3.BackgroundColor = Color.Green;
                return true;
            }
            else
            {
                btButton1.BackgroundColor = Color.Red;
                btButton2.BackgroundColor = Color.Red;
                btButton3.BackgroundColor = Color.Red;
                return false;
            }
        }

        private async void Continuar_Clicked(object sender, EventArgs e)
        {
            round++;
            if (ApertouTodos())
            {
                points++;
                var match = makeMatch(user.ID, jogo.ID, 1);
                getGame();
            }
            else
            {
                var match = makeMatch(user.ID, jogo.ID, 0);
                await Navigation.PushAsync(new GameOverPage(points, user));
            }
        }


    }
}