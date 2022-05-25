using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repaso_Net.Models
{ 
     [Table("t_detalle_compra")]
    public class DetalleCompra
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id {get; set;}
        public Curso curso {get; set;}
        public Decimal Precio { get; set; }
        public Compra compra {get; set;}
        
    }
}