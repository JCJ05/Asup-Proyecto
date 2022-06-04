using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Repaso_Net.Models
{
    [Table("t_tarjeta")]
    public class Tarjeta
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         [Column("id")]
        public int Id {get; set;}

        public String NombreTarjeta { get; set; }

         public String NumeroTarjeta { get; set; }
        
         public String DueDateYYMM { get; set; }
        
         public String Cvv { get; set; }

        public String NombreTitular { get; set; }
        public decimal Saldo { get; set; }

    }
}