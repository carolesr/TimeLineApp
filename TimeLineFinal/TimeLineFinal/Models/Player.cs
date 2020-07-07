using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace TimeLineFinal.Models
{
    public class Player : Entity<Player>
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        public override string ToString() => ID.ToString() + " - " + Username;

        public static async Task<List<Player>> All()
        {
            try
            {
                WebClient wc = new WebClient();
                using (var stream = await wc.OpenReadTaskAsync("http://timelinebe.azurewebsites.net/player/"))
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        string json = sr.ReadToEnd();
                        List<Player> lista_players = JsonConvert.DeserializeObject<List<Player>>(json);
                        return lista_players;

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
