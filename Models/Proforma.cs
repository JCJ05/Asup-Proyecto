using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Repaso_Net.Models
{   
     [Table("t_carrito")]
    public class Proforma
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        public Usuario usuario {get; set;}
        public Curso curso { get; set; }
        public string Status { get; set; } = "Pendiente";
    }
}