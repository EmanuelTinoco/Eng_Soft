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
    using System.ComponentModel.DataAnnotations;

    public partial class Utilizador
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utilizador()
        {
            this.Utilizador_Perfil = new HashSet<Utilizador_Perfil>();
        }

        public int id { get; set; }
        [Display(Name = "Codigo Postal:")]
        public string cod_postal { get; set; }
        [Display(Name = "Nome Completo:")]
        public string nome { get; set; }
        [Display(Name = "Numero Cartao Cidadao:")]
        public string cc { get; set; }
        [Display(Name = "Numero Cartao Eleitor:")]
        public string n_eleitor { get; set; }
        [Display(Name = "Email:")]
        public string email { get; set; }
        [Display(Name = "User Name:")]
        public string username { get; set; }
        [Display(Name = "Password:")]
        public string password { get; set; }
        [Display(Name = "Contacto:")]
        public int contacto { get; set; }

        public virtual Agendamento Agendamento { get; set; }
        public virtual Agendamento Agendamento1 { get; set; }
        public virtual Declaracao Declaracao { get; set; }
        public virtual Freguesia Freguesia { get; set; }
        public virtual Pedido_Declaracao Pedido_Declaracao { get; set; }
        public virtual Pedido_Esclarecimento Pedido_Esclarecimento { get; set; }
        public virtual Pedido_Esclarecimento Pedido_Esclarecimento1 { get; set; }
        public virtual Reportar_Problema Reportar_Problema { get; set; }
        public virtual Reportar_Problema Reportar_Problema1 { get; set; }
        public virtual Sugestao Sugestao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Utilizador_Perfil> Utilizador_Perfil { get; set; }
    }
}
