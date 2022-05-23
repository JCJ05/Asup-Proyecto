using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

<<<<<<< HEAD
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

=======
namespace Repaso_Net.Models
{
    [Table("DataPagos")]
    public class Pago
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        public DateTime fechaPago { get; set; } = DateTime.Now;
        public double monto { get; set; }
        public Usuario usuario { get; set; }
        public string estado { get; set; }
        public string metodoPago { get; set; }
        public string banco { get; set; }
        public string nombreCuenta { get; set; }
        public string nombrefile {get; set; }
         public string fileBase64 {get; set; }
        public Byte [] archivo {get; set;}

    }
>>>>>>> 6c7563f838572b214bab6c220bb12742b1c2cd44
}