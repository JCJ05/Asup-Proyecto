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
   
    public class CursoController : Controller {

        private readonly ApplicationDbContext _context;

        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<CursoController> _logger;

        public CursoController(ILogger<CursoController> logger  , ApplicationDbContext context , UserManager<Usuario> userManager) {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "administrador")]
        public IActionResult RegistrarCurso() {

            var usuarios = _userManager.GetUsersInRoleAsync("profesor").Result;
            ViewBag.items = usuarios;
            
            return View();
              
        }
        
         [Authorize(Roles = "administrador")]
         [HttpPost]
        public IActionResult RegistrarCurso(Curso curso , string profesor , List<IFormFile> files){
            var flag = false;

            if(ModelState.IsValid){

                if(files.Count > 0 ){

                    foreach(var file in files){

                       Console.WriteLine(Path.GetExtension(file.FileName).Substring(1));

                        if(Path.GetExtension(file.FileName).Substring(1) == "png" || Path.GetExtension(file.FileName).Substring(1) == "jpg"  || Path.GetExtension(file.FileName).Substring(1) == "jpeg" ){
                      
                          Stream str = file.OpenReadStream();
                          BinaryReader br = new BinaryReader(str);
                          Byte [] fileDet = br.ReadBytes((Int32) str.Length);
                          curso.archivo = fileDet;
                          curso.nombrefile = Path.GetFileName(file.FileName);
                          curso.fileBase64 = Convert.ToBase64String(fileDet);

                        }else {
                            
                            flag = true;
                            break;
                
                        }

 
                    }


                
                }

                if(flag == true){

                     var profesores = _userManager.GetUsersInRoleAsync("profesor").Result;
                     ViewBag.items = profesores;
                     ModelState.AddModelError("files" , "Solo se aceptan imagenes y no otro tipo de archivo");
                     return View(curso);
                }

                var profe = _context.DataUsuarios.Find(profesor);
                curso.usuario = profe;
                _context.Add(curso);
                _context.SaveChanges();
                
                return RedirectToAction("ListarCursos");

            }

        

             var usuarios = _userManager.GetUsersInRoleAsync("profesor").Result;
             ViewBag.items = usuarios;

            return View(curso);

        }


        public IActionResult ListarCursos(){

             var cursos = _context.DataCursos.Include(e => e.usuario).ToList();

             return View(cursos);
         }
         
         public IActionResult VerCurso(int id){


             var curso = _context.DataCursos.Include(e => e.usuario).Where(e => e.Id == id).FirstOrDefault();

             ViewData["Title"] = "Curso - " + curso.nombre;
             return View(curso);             

         }

          
          [Authorize(Roles = "administrador")]
         public IActionResult EditarCurso(int id){

               var curso = _context.DataCursos.Include(e => e.usuario).FirstOrDefault(s => s.Id == id);

               if(curso == null){

                   return NotFound();
               }

               var usuarios = _userManager.GetUsersInRoleAsync("profesor").Result;
               var coments =  usuarios.Where(e => e.Id != curso.usuario.Id).ToList();
               var usuario = _context.DataUsuarios.Find(curso.usuario.Id);
               
               ViewBag.items = coments;
               ViewBag.item = usuario;
               return View(curso);

         }

            
           [Authorize(Roles = "administrador")]
           [HttpPost]
         public IActionResult EditarCurso([Bind("Id , nombre , fechaInicio , fechafin , horario , cupo , informacion, precio , nombrefile , fileBase64")] Curso curso , string profesor , List<IFormFile> files){
            var flag = false;
            var usuarios = _userManager.GetUsersInRoleAsync("profesor").Result;
            var coments = usuarios.Where(e => e.Id != profesor).ToList();
            var usuario = _context.DataUsuarios.Find(profesor);
            

             if(ModelState.IsValid){

               if(files.Count > 0){

                    foreach(var file in files){

                       Console.WriteLine(Path.GetExtension(file.FileName).Substring(1));

                        if(Path.GetExtension(file.FileName).Substring(1) == "png" || Path.GetExtension(file.FileName).Substring(1) == "jpg"  || Path.GetExtension(file.FileName).Substring(1) == "jpeg" ){
                      
                          Stream str = file.OpenReadStream();
                          BinaryReader br = new BinaryReader(str);
                          Byte [] fileDet = br.ReadBytes((Int32) str.Length);
                          curso.archivo = fileDet;
                          curso.nombrefile = Path.GetFileName(file.FileName);
                          curso.fileBase64 = Convert.ToBase64String(fileDet);

                        }else {
                            
                            flag = true;
                            break;
                
                        }

 
                    }

                  

               }else {

                  var course = _context.DataCursos.AsNoTracking().Where(s => s.Id == curso.Id).FirstOrDefault();
                  curso.archivo = course.archivo;
                  curso.nombrefile = course.nombrefile;
                  curso.fileBase64 = course.fileBase64;


               }

                 if(flag == true){
                    
                    
                     ViewBag.items = coments;
                     ViewBag.item = usuario;
                     ModelState.AddModelError("files" , "Solo se aceptan imagenes y no otro tipo de archivo");
                     return View(curso);
                }

                
                 curso.usuario = usuario;
                 _context.Update(curso);
                 _context.SaveChanges();
                 return RedirectToAction("ListarCursos");

             }


            
             ViewBag.items = coments;
             ViewBag.item = usuario;
            
             return View(curso);
         }
         
           
          [Authorize(Roles = "administrador")]
          public IActionResult EliminarCurso(int id){
            
             var curso = _context.DataCursos.Find(id);

             if(curso == null){

                 return NotFound();
             }

             _context.DataCursos.Remove(curso);
             _context.SaveChanges();

            return RedirectToAction("ListarCursos");
       }
       
        
    }

}