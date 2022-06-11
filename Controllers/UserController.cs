
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repaso_Net.Data;
using Repaso_Net.Models;

namespace Repaso_Net.Controllers
{
    public class UserController: Controller 
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly SignInManager<Usuario> _signInManager;

        public UserController(ILogger<UserController> logger  , ApplicationDbContext context , UserManager<Usuario> userManager , SignInManager<Usuario> signInManager) {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        } 
        
        [HttpGet]
        public IActionResult EditarUsuario(){
              
            var usuario = _userManager.GetUserAsync(User).Result;
            return View(usuario);

        }

        [HttpPost]
        public async Task<ActionResult<String>> EditarUser([FromForm] string dni , [FromForm] string nombres , [FromForm] string apellidos , [FromForm] string direccion , [FromForm] string email) {
                
                var usuario = _userManager.GetUserAsync(User).Result;

               if(usuario == null){

                    return RedirectToAction("EditarUsuario","User");

               }else{

                    if(usuario.identificacion != dni){
                        usuario.identificacion = dni;
                    }

                    if(usuario.nombres != nombres){
                        usuario.nombres = nombres;
                    }

                    if(usuario.apellidos != apellidos){
                        usuario.apellidos = apellidos;
                    }

                    if(usuario.direccion != direccion){
                        usuario.direccion = direccion;
                    }

                    if(usuario.Email != email){
                        usuario.Email = email;
                    }

                
                    await _userManager.UpdateAsync(usuario);
                    await _signInManager.RefreshSignInAsync(usuario);

                    return new JsonResult(new { mensaje = "Usuario Actualizado con exito" });

               }
            
        }
    }

}