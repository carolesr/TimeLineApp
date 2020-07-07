using System;
using System.Collections.Generic;
using System.Text;
using TimeLineFinal.Models;

namespace TimeLineFinal.Models
{
    public class Match : Entity<Match>
    {
        public int PlayerID { get; set; }
        public int GameID { get; set; }
        public int Score { get; set; }
        
    }
}
