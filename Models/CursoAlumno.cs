using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repaso_Net.Models
{   
    [Table("t_curso_alumno")]
    public class CursoAlumno
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public Usuario usuario { get; set; }

        public Curso Curso { get; set; }

        public DateTime fechaMatricula { get; set; }

        
    }
}