using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

<<<<<<< HEAD
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
=======

namespace Repaso_Net.Models {
  
    [Table("t_pago")]
   public class Pago {

       
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         [Column("id")]
         public int Id { get; set; }

         public DateTime fechaPago { get; set; }
         
         [Required(ErrorMessage = "El monto es requerido")]
         public decimal monto { get; set; }

         public Usuario usuario { get; set; }

         public String NombreTarjeta { get; set; }
        
         [NotMapped]
         public String NumeroTarjeta { get; set; }
        
         [NotMapped]
         [Required(ErrorMessage = "El campo DueDate es obligatorio")]
         [MinLength(5, ErrorMessage = "El campo DueDate debe tener 5 caracteres")]
         public String DueDateYYMM { get; set; }
        
         [NotMapped]
         [Required(ErrorMessage = "El campo CVV es obligatorio")]
         [MinLength(3, ErrorMessage = "El campo CVV debe tener 3 caracteres")]
         public String Cvv { get; set; }

         public String modalidad { get; set; }

         public string nombreArchivo {get; set; }

         public string fileVoucher {get; set; }

         public Byte [] voucher {get; set;}

         public string Status { get; set; }
   }

>>>>>>> 43dd4014a2a238d8f681cd324fa80b5bfafbda86
}