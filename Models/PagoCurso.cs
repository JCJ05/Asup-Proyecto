using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repaso_Net.Models {
  
   public class PagoCurso {

       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public Pago pago { get; set; }

        public Curso curso { get; set; }
   }

}