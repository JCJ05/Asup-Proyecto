using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Repaso_Net.Data;
using Repaso_Net.Models;
using System.Linq;
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
        public async Task<ActionResult<string>> GetFiltros(string nombre, decimal precioMin, decimal precioMax, string fechaMin, string fechaMax)
        {  

               var cursos = await _context.DataCursos.ToListAsync();

                var query1 = cursos;
                var query2 = cursos;
                var query3 = cursos;

                if(nombre != "null"){
                    query1 = query1.Where(x => x.nombre.ToLower().Contains(nombre.ToLower())).ToList();
                }

                if(precioMin != 0 || precioMax != 0){
                    if(precioMin == 0){
                        query2 = query2.Where(x => x.precio <= precioMax).ToList();
                    }else if(precioMax == 0){
                        query2 = query2.Where(x => x.precio >= precioMin).ToList();
                    }else{
                        query2 = query2.Where(x => x.precio >= precioMin && x.precio <= precioMax).ToList();
                    }
                }

                if(fechaMin != "no" || fechaMax != "no"){
                    if(fechaMin == "no"){
                        query3 = query3.Where(x => x.fechaInicio <= DateTime.ParseExact(fechaMax, "yyyy-MM-dd", null)).ToList();
                    }else if(fechaMax == "no"){
                        query3 = query3.Where(x => x.fechaInicio >= DateTime.ParseExact(fechaMin, "yyyy-MM-dd", null)).ToList();
                    }else{
                        query3 = query3.Where(x => x.fechaInicio >= DateTime.ParseExact(fechaMin, "yyyy-MM-dd", null) && x.fechaInicio <= DateTime.ParseExact(fechaMax, "yyyy-MM-dd", null)).ToList();
                    }
                }

            /*
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
             */
            var resultado = query1.Intersect(query2).Intersect(query3).ToList();

           return new JsonResult(new { cursosFiltro = resultado });
           
        }

    
    }
}