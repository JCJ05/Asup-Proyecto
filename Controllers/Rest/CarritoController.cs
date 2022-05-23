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
    [Route("api/carrito")]
    public class CarritoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CarritoController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        [HttpGet]
       public async Task<ActionResult<String>> getCurso(int id){
            var curso = await _context.DataCursos.FindAsync(id);
            return new JsonResult(new { course = curso });
        
       }
    }
}