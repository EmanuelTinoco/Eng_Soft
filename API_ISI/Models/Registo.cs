using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_ISI.Models
{
    public class Registo
    {
        public int Id { get; set; }
        public int Userid { get; set; }
        public DateTime Data_Hora_Login { get; set; }
        public DateTime Data_Hora_Logoff { get; set; }
    }
}