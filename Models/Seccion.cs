
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repaso_Net.Models
{
    [Table("t_seccion")]
    public class Seccion {
   
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string titulo { get; set; }

        public string subtitulo { get; set; }

        public string linkClase { get; set; }

        public Module module { get; set; }

        public List<Archivo> archivos { get; set; }



    }

}
