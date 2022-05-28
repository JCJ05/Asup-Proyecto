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

=======
          public DbSet<Repaso_Net.Models.Proforma> DataProformas {get; set; }
>>>>>>> 43dd4014a2a238d8f681cd324fa80b5bfafbda86
          public DbSet<Repaso_Net.Models.Pago> DataPagos {get; set; }
          public DbSet<Repaso_Net.Models.Compra> DataCompras {get; set; }
          public DbSet<Repaso_Net.Models.DetalleCompra> DataDetalleCompras {get; set; }
          public DbSet<Repaso_Net.Models.CursoAlumno> DataCursoAlumnos {get; set; }

<<<<<<< HEAD
          public DbSet<Repaso_Net.Models.PagoCurso> DataPagoCursos {get; set; }
=======
>>>>>>> 43dd4014a2a238d8f681cd324fa80b5bfafbda86
    }
}
