//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sugestao
    {
        public int id_user { get; set; }
        public System.DateTime data { get; set; }
        public string descricao { get; set; }
        public int id { get; set; }
    
        public virtual Utilizador Utilizador { get; set; }
    }
}
