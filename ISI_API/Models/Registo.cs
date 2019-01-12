using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ISI_API.Models
{
    [DataContract]
    public class Registo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Userid { get; set; }
        [DataMember]
        public DateTime Data_Hora_Login { get; set; }
        [DataMember]
        public DateTime Data_Hora_Logoff { get; set; }
    }
}
