using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EG.Models
{
    public class Permicoes
    {
        public int Id { get; set; }
        public bool Admin { get; set; }
        public bool Residente { get; set; }
        public bool Membro { get; set; }
    }
}