

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repaso_Net.Data;
using Repaso_Net.Models;

namespace Repaso_Net.Controllers
{

    public class ModuleController: Controller 
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<CursoController> _logger;

        public ModuleController(ILogger<CursoController> logger  , ApplicationDbContext context , UserManager<Usuario> userManager) {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

         [Authorize(Roles = "profesor,alumno")]
         public IActionResult verInfoCurso(int id , int idModulo = 0){
             var idFind = 0;
             var curso = _context.DataCursos.Include(x => x.usuario).Where(x => x.Id == id).FirstOrDefault();
             
             if(curso == null){
                 
                    return RedirectToAction("MisCursos", "Curso");
             }

             if(User.IsInRole("alumno")){
                 var idUser = _userManager.GetUserId(User);
                 var cursoUser = _context.DataCursoAlumnos.Include(x => x.usuario).Include(x=> x.Curso).Where(x => x.usuario.Id == idUser && x.Curso.Id == id).FirstOrDefault();
               
                
                 if(cursoUser == null){
                     return RedirectToAction("MisCursos", "Curso");
                 }

                 if(idUser != _userManager.GetUserId(User)){
                     return RedirectToAction("MisCursos", "Curso");
                 }

             }

             var modulos = _context.DataModules.Include(x => x.curso).Where(x => x.curso.Id == id).ToList();
            
             if(idModulo == 0){

                 var idModulos = _context.DataModules.Include(x => x.curso).Where(x => x.curso.Id == id).FirstOrDefault();
                  
                if(idModulos != null){

                    idFind = idModulos.id;

                }
                

             }else {
                    
                    var modulo = _context.DataModules.Include(x => x.curso).Where(x => x.id == idModulo && x.curso.Id == id).FirstOrDefault();

                    if(modulo != null){

                        idFind = modulo.id;

                    }else {

                        idFind = _context.DataModules.Include(x => x.curso).Where(x => x.curso.Id == id).FirstOrDefault().id;
                        
                    }

             }

             _logger.LogInformation("idFind: " + idFind);
             var secciones = _context.DataSecciones.Include(x => x.module).Where(x => x.module.id == idFind).ToList();
            

             foreach(var item in secciones){
                 item.archivos = _context.DataArchivos.Include(x => x.seccion).Where(x => x.seccion.Id == item.Id).ToList();
             }
            
             var usuarioId = _userManager.GetUserId(User);

             if(usuarioId == curso.usuario.Id){

                 ViewBag.isOwner = true;
                 
             }else {

                    ViewBag.isOwner = false;
            }

             ViewBag.idSeccion = idFind;
             ViewBag.modulos = modulos;
             ViewBag.secciones = secciones;

             return View(curso);

         }
         
         [HttpPost]
         public async Task<ActionResult<String>> saveArchivos([FromForm] string titulo , [FromForm] string subtitulo , [FromForm] string link , [FromForm] int cursoId , [FromForm] int moduloId , [FromForm] IFormFile[] archivos )
         {     
  
                var seccion = new Seccion();
                seccion.titulo = titulo;
                seccion.subtitulo = subtitulo;
                seccion.linkClase = link;
                seccion.module = _context.DataModules.Where(x => x.id == moduloId).FirstOrDefault();
                await _context.AddAsync(seccion);
                
                List<Archivo> archivosList = new List<Archivo>();
                foreach(var file in archivos){

                    _logger.LogInformation(Path.GetExtension(file.FileName).Substring(1));
                   
                    Stream str = file.OpenReadStream();
                    BinaryReader br = new BinaryReader(str);
                    Byte [] fileDet = br.ReadBytes((Int32) str.Length);

                    var archivo = new Archivo();
                    archivo.nombreArchivo = Path.GetFileName(file.FileName);
                    archivo.seccion = seccion;
                    archivo.extension = Path.GetExtension(file.FileName).Substring(1);
                    archivo.archivo = fileDet;
                    archivosList.Add(archivo);
    
                }

               

                await _context.AddRangeAsync(archivosList);
                await _context.SaveChangesAsync();
    
                 return new JsonResult(new { mensaje = "Pago Aprobado" });

         }

           public IActionResult DownLoadArchivo(int id){
        
           var document = _context.DataArchivos.Where(x => x.Id == id).FirstOrDefault();
           var file = document.archivo;
           var fileName = document.nombreArchivo;
           var contentType = "application/" + document.extension;


           return File(file, contentType, fileName);
          
       }

       [HttpGet]
        public async Task<ActionResult<Seccion>> getSeccionById(int id){

            var seccion = await _context.DataSecciones.Where(x => x.Id == id).FirstOrDefaultAsync();
            //var archivos = _context.DataArchivos.Include(x => x.seccion).Where(x => x.seccion.Id == id).ToList();
            
            return new JsonResult(new { seccion = seccion  });
    
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Archivo>>>  getArchivosBySeccion(int id){

            var archivos = await _context.DataArchivos.Where(x => x.seccion.Id == id).ToListAsync();

            return archivos;
    
        }

        [HttpPost]
        public async Task<ActionResult<String>> updateSeccion( [FromForm] string titulo , [FromForm] string subtitulo , [FromForm] string link , [FromForm] int idSeccion  ,[FromForm] IFormFile[] archivos , [FromForm] List<Archivo> listArchivos){

            var seccion = await _context.DataSecciones.Include(x => x.module).Where(x => x.Id == idSeccion).FirstOrDefaultAsync();
            seccion.titulo = titulo;
            seccion.subtitulo = subtitulo;
            seccion.linkClase = link;
            seccion.archivos = new List<Archivo>();
            
             _context.Update(seccion);

            var cursoId = _context.DataModules.Include(x => x.curso).Where(x => x.id == seccion.module.id).FirstOrDefault().curso.Id;

            List<Archivo> archivosList = new List<Archivo>();

            foreach(var file in archivos){
               
                Stream str = file.OpenReadStream();
                BinaryReader br = new BinaryReader(str);
                Byte [] fileDet = br.ReadBytes((Int32) str.Length);

                var archivo = new Archivo();
                archivo.nombreArchivo = Path.GetFileName(file.FileName);
                archivo.seccion = seccion;
                archivo.extension = Path.GetExtension(file.FileName).Substring(1);
                archivo.archivo = fileDet;
                archivosList.Add(archivo);

            }

            await _context.AddRangeAsync(archivosList);
            await _context.SaveChangesAsync();

            return new JsonResult(new { cursoId = cursoId , moduloId = seccion.module.id });

        }

        [HttpGet]
        public async Task<ActionResult<String>> deleteArchivo(int id){

            var archivo = await _context.DataArchivos.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.Remove(archivo);
            await _context.SaveChangesAsync();

            return new JsonResult(new { mensaje = "Archivo Eliminada" });

        }

        [HttpGet]
        public async Task<ActionResult<String>> deleteSeccion(int id){
            
            var archivosOld = _context.DataArchivos.Where(x => x.seccion.Id == id).ToList();
             _context.RemoveRange(archivosOld);
            var seccion = await _context.DataSecciones.Where(x => x.Id == id).FirstOrDefaultAsync();

           
            _context.DataSecciones.Remove(seccion);
            await _context.SaveChangesAsync();

            return new JsonResult(new { mensaje = "Seccion Eliminada" });

        }

    }



}