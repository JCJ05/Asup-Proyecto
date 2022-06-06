
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repaso_Net.Data;
using Repaso_Net.Models;

namespace Repaso_Net.Controllers
{

    public class InscripcionesController: Controller

    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<CursoController> _logger;

        public InscripcionesController(ILogger<CursoController> logger  , ApplicationDbContext context , UserManager<Usuario> userManager) {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        
        [Authorize(Roles = "administrador")]
        public IActionResult verCursos(){
              
              var cursos = _context.DataCursos.Include(x => x.usuario).ToList();
              return View(cursos);

        }

        [Authorize(Roles = "administrador")]
        public IActionResult verInscritosByCurso(int id){

            var inscritos = _context.DataCursoAlumnos.Include(x => x.usuario).Include(x => x.Curso).Where(x => x.Curso.Id == id).ToList();
            var curso = _context.DataCursos.FirstOrDefault(x => x.Id == id);

            ViewBag.curso = curso;
            return View(inscritos);
        }

        public async Task<ActionResult<String>> findCursosByNombre(string nombre){
           
           var cursos = new List<Curso>();

            if(nombre != null){
                 
                  cursos = await _context.DataCursos.Include(x => x.usuario).Where(x => x.nombre.ToLower().Contains(nombre.ToLower())).ToListAsync();

            }else {
                 
                   cursos = await _context.DataCursos.Include(x => x.usuario).ToListAsync();
            }

           
            
            if(cursos.Count == 0){

                return BadRequest("No se encontraron cursos");
            }

            return new JsonResult(new {  dataCursos = cursos});
        }


        
    }
    
}