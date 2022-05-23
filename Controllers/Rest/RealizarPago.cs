
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Repaso_Net.Models;
using Repaso_Net.Data;


namespace Repaso_Net.Controllers.Rest {
   
    [ApiController]
    [Route("api/pago")]
    public class RealizarPagoController : Controller {

        private readonly ApplicationDbContext _context;

       
        private readonly ILogger<CursoController> _logger;

         private readonly UserManager<Usuario> _userManager;

        public RealizarPagoController(ILogger<CursoController> logger  , ApplicationDbContext context  , UserManager<Usuario> userManager) {
            _logger = logger;
            _context = context;
            _userManager = userManager;
           
        }

        
        
        [HttpPost]
        public IActionResult CrearPago(List<Curso> cursos , int precio) {
            
            _logger.LogInformation(cursos.Count.ToString());
            var user = _userManager.GetUserAsync(User);
            var pago = new Pago();
            pago.fechaPago = DateTime.Now;
            pago.usuario = user.Result;
            pago.monto = precio;
            pago.estado = "Procesado";
  
            decimal monto = 0;

              _context.DataPagos.Add(pago);

            var cursosPago = new List<PagoCurso>();             

            foreach (var curso in cursos.ToList()) {
                 
                 _logger.LogInformation(curso.nombre);

                var pagoCurso = new PagoCurso();
                pagoCurso.curso = _context.DataCursos.FirstOrDefault(c => c.Id == curso.Id);
                pagoCurso.pago = pago;
                monto+=curso.precio;
                cursosPago.Add(pagoCurso);

        
          }

            _context.DataPagoCursos.AddRange(cursosPago);
            _context.SaveChanges();

            return Ok();
              
        }
        
       
       
        
    }

}