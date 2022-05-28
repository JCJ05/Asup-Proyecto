using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Repaso_Net.Models
{
      
       [Table("t_curso")]
      public class Curso {
    
           
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string nombre { get; set; }

        public DateTime fechaInicio { get; set; }

        public DateTime fechaFin { get; set; }

        public string horario { get; set; }

        public int cupo { get; set; }

        public Usuario usuario { get; set; }

        public string informacion {get; set;}

        public decimal precio { get; set; }

        public string nombrefile {get; set; }

        public string fileBase64 {get; set; }

        public Byte [] archivo {get; set;}

          
       

      }


}