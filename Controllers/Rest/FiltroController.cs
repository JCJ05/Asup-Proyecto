using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Repaso_Net.Data;
using Repaso_Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Repaso_Net.Controllers.Rest
{
    [ApiController]
    [Route("api/filtro")]
    public class FiltroController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public FiltroController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetFiltros(string nombre = null , decimal precio = 0 , string fecha = null )
        {  

               var cursos = await _context.DataCursos.ToListAsync();
        

            if(nombre != "null" && precio == 0 && fecha == "no"){
                cursos = cursos.FindAll(x => x.nombre.ToLower().Contains(nombre.ToLower()));
            }else if(nombre == "null" && precio != 0 && fecha == "no"){
                cursos = cursos.FindAll(x => x.precio == precio);
            }else if(nombre == "null" && precio == 0 && fecha != "no"){
                cursos = cursos.FindAll(x => x.fechaInicio.ToString("yyyy-MM-dd") == (fecha));
            }else if(nombre != "null" && precio != 0 && fecha == "no"){
                cursos = cursos.FindAll(x => x.nombre.ToLower().Contains(nombre.ToLower()) && x.precio == precio);
            }else if(nombre != "null" && precio == 0 && fecha != "no"){
                cursos = cursos.FindAll(x => x.nombre.ToLower().Contains(nombre.ToLower()) && x.fechaInicio.ToString("yyyy-MM-dd") == (fecha));
            }else if(nombre == "null" && precio != 0 && fecha != "no"){
                cursos = cursos.FindAll(x => x.precio == precio && x.fechaInicio.ToString("yyyy-MM-dd") == (fecha));
            }else if(nombre != "null" && precio != 0 && fecha != "no"){
                cursos = cursos.FindAll(x => x.nombre.ToLower().Contains(nombre.ToLower()) && x.precio == precio && x.fechaInicio.ToString("yyyy-MM-dd") == (fecha));
            }
             
           return new JsonResult(new { cursosFiltro = cursos });
           
        }

        /*[HttpGet]
        public async Task<ActionResult<string>> GetTodoItems(string nombre, string apellido, string identificacion)
        {
            var usuarios = _context.Usuario.Where(x => x.nombre.Contains(nombre) && x.apellido.Contains(apellido) && x.identificacion.Contains(identificacion));
            if (usuarios == null)
            {
                return NotFound($"No existe el usuario con el nombre:  '{nombre}'.");
            }

            return Ok(usuarios);
        }*/
    }
}