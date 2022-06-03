
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repaso_Net.Models
{
    
    [Table("t_archivo")]
    public class Archivo {


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string nombreArchivo { get; set; }

        public string extension { get; set; }

        public Byte[] archivo { get; set; }

        public Seccion seccion { get; set; }

    }

}