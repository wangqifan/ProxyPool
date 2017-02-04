using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyPool
{
    public class Proxy
    {
        [Key]
        public string Adress { get; set; }
        public int port { get; set; }
    }
}
