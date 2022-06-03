using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


 namespace Repaso_Net.Models
{
    
     [Table("t_modulo")]
    public class Module {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
      
        public string nombre { get; set; }

        public Curso curso { get; set; }

        public List<Seccion> secciones { get; set; }

    }
    
}