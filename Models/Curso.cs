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
        
        [Required(ErrorMessage = "El nombre del curso es requerido")]
        public string nombre { get; set; }
        
        [Required(ErrorMessage = "La fecha de inicio del curso es requerida")]
        public DateTime fechaInicio { get; set; }
        
        [Required(ErrorMessage = "La fecha de fin del curso es requerida")]
        public DateTime fechaFin { get; set; }
        
        [Required(ErrorMessage = "El horario del profesor es requerido")]
        public string horario { get; set; }
        
        [Required(ErrorMessage = "El cupo es requerido")]
        public int cupo { get; set; }

        public Usuario usuario { get; set; }
        
        [Required(ErrorMessage = "La informacion del curso es requerido")]
        public string informacion {get; set;}
        
        [Required(ErrorMessage = "El precio del curso es requerido")]
        public decimal precio { get; set; }

        public string nombrefile {get; set; }

        public string fileBase64 {get; set; }

        public Byte [] archivo {get; set;}

          
       

      }


}