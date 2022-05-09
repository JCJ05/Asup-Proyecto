using System;
using Microsoft.AspNetCore.Identity;

namespace Repaso_Net.Models {
  
   public class Usuario : IdentityUser {

       [PersonalData]
        public string nombres { get; set; }
        [PersonalData]
        public string apellidos { get; set; }

        [PersonalData]
        public string identificacion { get; set; }

        [PersonalData]
        public string direccion { get; set; }

        
   }

}
 