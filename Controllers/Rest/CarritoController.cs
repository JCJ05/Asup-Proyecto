using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Repaso_Net.Data;
using Repaso_Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Repaso_Net.Controllers.Rest
{   
    
    [ApiController]
    [Route("api/carrito")]
    public class CarritoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
    
        public CarritoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proforma>>> GetCarrito()

        {

            string mensaje = "No hay nada";

            if(User.Identity.IsAuthenticated)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                var carrito = await _context.DataProformas.Include(p => p.curso).Where(p => p.usuario.Id == user.Id && p.Status.Equals("Pendiente")).ToListAsync();
                return carrito;
            }
            else
            {
                return BadRequest(mensaje);
            }

        }
      
       [HttpPut]
       public async Task<ActionResult<String>> addCurso(int id){
           
             
             if(User.Identity.IsAuthenticated){

                  var curso = await _context.DataCursos.FirstOrDefaultAsync(c => c.Id == id);
                  var user = await _context.DataUsuarios.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                  string mensaje = "";

                   if(curso!=null && user!=null){

                        var verificarCurso = await _context.DataCursoAlumnos.FirstOrDefaultAsync(ca => ca.Curso.Id == curso.Id && ca.usuario.Id == user.Id);
                        
                        if(verificarCurso == null){

                            var proforma = await _context.DataProformas.FirstOrDefaultAsync(p => p.curso.Id == id && p.usuario.Id == user.Id && p.Status.Equals("Pendiente"));
                            if(proforma == null){

                                var proformaNueva = new Proforma();
                                proformaNueva.curso = curso;
                                proformaNueva.usuario = user;
                                proformaNueva.Status = "Pendiente";
                                _context.DataProformas.Add(proformaNueva);
                                await _context.SaveChangesAsync();
                                mensaje = "Curso agregado al carrito";

                            }else{

                                mensaje = "El curso ya esta en el carrito";

                            }

                        }else{

                            mensaje = "El usuario ya esta inscrito en el curso";

                        }
                            

                    }else if(curso==null){

                        mensaje = "Curso no encontrado";

                    }else if(user==null){

                        mensaje = "Usuario no encontrado";

                    }else{

                        mensaje = "Error desconocido";

                    }
                 
                    return new JsonResult(new { course = mensaje });

             }else {

                return BadRequest("No estas autenticado");
             }

       }

           [HttpDelete]
            public async Task<ActionResult<String>> deleteCurso(int id){
    
                if(User.Identity.IsAuthenticated){
    
                    var proforma = await _context.DataProformas.FirstOrDefaultAsync(p => p.Id == id);
                    var user = await _context.DataUsuarios.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                    string mensaje = "";
    
                    if(proforma!=null && user!=null){
    
                        if(proforma.usuario.Id == user.Id){
    
                            _context.DataProformas.Remove(proforma);
                            mensaje = "Curso eliminado del carrito";
                             await _context.SaveChangesAsync();
    
                        }else{
    
                            mensaje = "No puedes eliminar este curso";
    
                        }
    
                    }else if(proforma==null){
    
                        mensaje = "Curso no encontrado";
    
                    }else if(user==null){
    
                        mensaje = "Usuario no encontrado";
    
                    }else{
    
                        mensaje = "Error desconocido";
    
                    }
    
                    return new JsonResult(new { course = mensaje });
    
                }else{
    
                    return BadRequest("No estas autenticado");
                }
    
            }

    }

}