
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
    public class AdminController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger  , ApplicationDbContext context , UserManager<Usuario> userManager) {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "administrador")]
        public IActionResult getRolUsers(){

             var usersWithRolUsuario = _userManager.GetUsersInRoleAsync("alumno").Result; 

             return View(usersWithRolUsuario);            

        }

        public async Task<ActionResult<String>> changeRolUser(string id){
                
                var user = await _userManager.FindByIdAsync(id);
    
                if(user == null){
                    return RedirectToAction("getRolUsers");
                }
    
                var result = await _userManager.RemoveFromRoleAsync(user, "alumno");
    
                if(!result.Succeeded){
                    return BadRequest();
                }
    
                result = await _userManager.AddToRoleAsync(user, "profesor");
    
                if(!result.Succeeded){
                    return BadRequest();
                }
    
                return new JsonResult(new {  mensaje = "Rol cambiado correctamente" });
        }
        
        [HttpGet]
        public async Task<ActionResult<String>> findByDni(string dni){
              
              var rolAlumno = _userManager.GetUsersInRoleAsync("alumno").Result;

              List<Usuario> users = new List<Usuario>();

             foreach(var user in rolAlumno){

                if(dni != null){

                    if(user.identificacion.Contains(dni)){

                        users.Add(user);                  
    
                    }

                }else {

                    users.Add(user);

                }

             }

             if(users.Count == 0){

                return BadRequest("No se encontraron usuarios");
            
             }


              return new JsonResult(new {  dataUsuarios = users });

        }
    }
    
}