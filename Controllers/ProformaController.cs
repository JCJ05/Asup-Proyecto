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


namespace Repaso_Net.Controllers {
   
    public class ProformaController : Controller {

        private readonly ApplicationDbContext _context;

        private readonly ILogger<CursoController> _logger;

        private readonly UserManager<Usuario> _userManager;

        public ProformaController(ILogger<CursoController> logger  , ApplicationDbContext context , UserManager<Usuario> userManager) {

            _logger = logger;
            _context = context;
            _userManager = userManager;
           
        }

        [Authorize(Roles = "profesor,alumno")]
        public IActionResult MostrarItems() {
            
            var user = _userManager.GetUserAsync(User);
            var proformas = _context.DataProformas.Include(p => p.curso).Where(p => p.usuario.Id == user.Result.Id && p.Status.Equals("Pendiente")).ToList();
             
            decimal sueldo = 0;

            foreach(var proforma in proformas) {
                sueldo += proforma.curso.precio;
            }

            ViewData["Title"] = "Carrito";
            ViewBag.sueldo = sueldo;
            
            return View(proformas);
              
        }
        
       
       
        
    }

}