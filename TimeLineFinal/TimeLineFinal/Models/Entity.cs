using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace TimeLineFinal.Models
{
    public abstract class Entity<T> where T : Entity<T>, new()
    {
        public int ID { get; set; } = 0;
    }
}
