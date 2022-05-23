using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repaso_Net.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

          public DbSet<Repaso_Net.Models.Usuario> DataUsuarios {get; set; }
          public DbSet<Repaso_Net.Models.Curso> DataCursos {get; set; }
<<<<<<< HEAD

          public DbSet<Repaso_Net.Models.Pago> DataPagos {get; set; }

          public DbSet<Repaso_Net.Models.PagoCurso> DataPagoCursos {get; set; }
=======
          public DbSet<Repaso_Net.Models.Pago> DataPagos {get; set; }
>>>>>>> 6c7563f838572b214bab6c220bb12742b1c2cd44
    }
}
