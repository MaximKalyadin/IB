using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IB1.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsBlock { get; set; }
        public bool IsLimit { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
