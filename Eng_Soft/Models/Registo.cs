using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng_Soft.Models
{
    public class Registo
    {
        public int ID { get; set; }
        public int Userid { get; set; }
        public DateTime Data_Hora_Login { get; set; }
        public DateTime Data_Hora_Logoff { get; set; }
    }
}
