using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repaso_Net.Models
{
     [Table("t_compra")]
    public class Compra
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        public Usuario usuario { get; set; }
        public Decimal Total { get; set; } 
        public Pago Pago { get; set; }
        public Byte [] boleta {get; set;}
    }
}