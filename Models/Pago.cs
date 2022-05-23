using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repaso_Net.Models {
  
   public class Pago {

       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

         public DateTime fechaPago { get; set; }

        public decimal monto { get; set; }

        public Usuario usuario { get; set; }

        public string estado { get; set; }
   }

}