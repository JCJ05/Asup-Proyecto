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
using DinkToPdf;
using DinkToPdf.Contracts;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Repaso_Net.Models;
using Repaso_Net.Data;
using IronPdf;

namespace Asup_Proyecto.Controllers
{
    public class PagoController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<PagoController> _logger;
        private readonly IConverter _converter;

        public PagoController(ILogger<PagoController> logger, ApplicationDbContext context, UserManager<Usuario> userManager , IConverter converter)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _converter = converter;
        }

        [Authorize(Roles = "profesor,alumno")]
        public IActionResult IndexPago()
        {
            var user = _userManager.GetUserAsync(User);
            var proformas = _context.DataProformas.Include(p => p.curso).Where(p => p.usuario.Id == user.Result.Id && p.Status.Equals("Pendiente")).ToList();

            decimal sueldo = 0;

            if(proformas.Count > 0)
            {
                foreach (var proforma in proformas)
                {
                    sueldo += proforma.curso.precio;
                }
            }
            
            ViewBag.proformas = proformas;
            ViewBag.monto = sueldo;

            ViewData["Title"] = "Pago con tarjeta";
            return View();
        }
        
        
        [Authorize(Roles = "profesor,alumno")]
        [HttpPost]
        public IActionResult IndexPago(Pago pago , string tipoTarjeta){
              
              if(ModelState.IsValid){

                   var user = _userManager.GetUserAsync(User);
                   var proformas = _context.DataProformas.Include(p => p.curso).Where(p => p.usuario.Id == user.Result.Id && p.Status.Equals("Pendiente")).ToList();
                   var montototal = proformas.Sum(c => c.curso.precio);

                   var boleta = GeneratePdfReport(proformas);

                   pago.NombreTarjeta = tipoTarjeta;
                   pago.Status = "Realizado";                   
                   pago.usuario = user.Result;
                   pago.fechaPago = DateTime.Now;
                   pago.modalidad = "Tarjeta";
                   pago.monto = montototal;
                   _context.Add(pago);

                   Compra compra = new Compra();
                   compra.usuario = user.Result;
                   compra.Total = pago.monto;
                   compra.Pago = pago;
                   compra.boleta = boleta;
                   _context.Add(compra);

                   List<DetalleCompra> detalles = new List<DetalleCompra>();
                   List<CursoAlumno> cursoAlumnos = new List<CursoAlumno>();
                   
                     foreach (var proforma in proformas)
                     {
                          DetalleCompra detalle = new DetalleCompra();
                          detalle.curso = _context.DataCursos.FirstOrDefault(c => c.Id == proforma.curso.Id);
                          detalle.compra = compra;
                          detalle.Precio = _context.DataCursos.FirstOrDefault(c => c.Id == proforma.curso.Id).precio;
                          

                          CursoAlumno cursoAlumno = new CursoAlumno();
                          cursoAlumno.Curso = _context.DataCursos.FirstOrDefault(c => c.Id == proforma.curso.Id);
                          cursoAlumno.usuario = user.Result;
                          cursoAlumno.fechaMatricula = DateTime.Now;


                          proforma.Status = "Realizado";

                          var curso = _context.DataCursos.FirstOrDefault(c => c.Id == proforma.curso.Id);
                          curso.cupo = curso.cupo - 1;
                           
                          _context.Update(curso);
                          

                          detalles.Add(detalle);
                          cursoAlumnos.Add(cursoAlumno);
                     }

                        _context.AddRange(detalles);
                        _context.AddRange(cursoAlumnos);
                        _context.UpdateRange(proformas);

                        _context.SaveChanges();

                        return RedirectToAction("MisPedidos");

              }


              
            var usuario = _userManager.GetUserAsync(User);
            var carritos = _context.DataProformas.Include(p => p.curso).Where(p => p.usuario.Id == usuario.Result.Id && p.Status.Equals("Pendiente")).ToList();

            decimal sueldo = 0;

            if(carritos.Count > 0)
            {
                foreach (var proforma in carritos)
                {
                    sueldo += proforma.curso.precio;
                }
            }
            
                ViewBag.proformas = carritos;
                ViewBag.monto = sueldo;


                return View();

        }

        [Authorize(Roles = "profesor,alumno")]
        public IActionResult MisPedidos()
        {
            var user = _userManager.GetUserAsync(User);
            var compras = _context.DataCompras.Include(c => c.Pago).Include(c => c.usuario).Where(c => c.usuario.Id == user.Result.Id).ToList();

            ViewData["Title"] = "Pedidos";
            return View(compras);
        }

        [Authorize(Roles = "profesor,alumno")]
        public IActionResult MisPedidosPendientes()
        {
            var user = _userManager.GetUserAsync(User);
            var pagos = _context.DataPagos.Include(p => p.usuario).Where(p => p.usuario.Id == user.Result.Id && p.Status.Equals("Pendiente")).ToList();

            ViewData["Title"] = "Pedidos Pendientes";
            return View(pagos);

        }

        public byte[] GeneratePdfReport(List<Proforma> proformas)
    {   

            var total = proformas.Sum(c => c.curso.precio);
            var oneProfoma = proformas.FirstOrDefault();
            var usuario = _context.DataUsuarios.FirstOrDefault(u => u.Id == oneProfoma.usuario.Id);

            var cabecera = $@"
            
            <!DOCTYPE html>
                <html lang=""en"">
                <head>
                <meta charset=""UTF-8"">
                <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC"" crossorigin=""anonymous"">
                <link rel=""stylesheet"" href=""~/css/pdf.css"">
                <title>Reporte de Pago</title>
            </head>
            <body>
        
            <div class=""container mt-10"">
            <div class=""flex-header"">
                <img src=""~/img/logo.png"" alt="""">
                <div class=""texto"">
                <p> <span clas=""fw-bold"">N° Boleta: </span> 9484848484</p>
                <p><span clas=""fw-bold"">Direccion: </span> Jr Ocaña 349</p>
                <p><span clas=""fw-bold"">Ruc: </span> 39393939</p>
                </div>
            </div>
            <div class=""card mb-5"">
                    <div class=""card-header"">
                    Resumen de Compra
                    </div>
                    <div class=""card-body"">
                    <h5 class=""card-title"">Gracias por Confiar en nosotros</h5>
                    <p class=""card-text"">Cualquier consulta contactarnos al: 983878398</p>
                    </div>
                </div>
            <table class=""table table-striped"">
            <thead>
                <tr>
                <th>Nombre</th>
                <th>Imagen</th>
                <th>Precio</th>
                </tr>
            </thead>
            <tbody>
            
            ";

            var nombre = "";
            var precio = new decimal();

            proformas.ForEach( item => {
                
                nombre = item.curso.nombre;
                precio = item.curso.precio;

                var cuerpoTabla = $@"
                
                    <tr>
                    
                    <td>{nombre}</td>
                    <td><img width=""30%"" src=""data:image/png;base64,{item.curso.fileBase64}""></td>
                    <td>{precio}</td>
                    </tr>
                
                "; 

                cabecera+=cuerpoTabla;    
        

            });


            var footer = $@"
            
                </tbody>
                </table>
                <div class=""mt-5"">
                <p class=""text-end fs-1"">El monto a pagar fue: S./{total}</p>
            
            
            </div>
            </div>
            
            </body>
        </html>
            
            ";

            cabecera += footer; 
            //sendBoletaToClient(cabecera , usuario.Email);

        var renderer = new ChromePdfRenderer();
        var pdfDocument = renderer.RenderHtmlAsPdf(cabecera);

        return pdfDocument.BinaryData;

      }

      public void sendBoletaToClient(string html , string email){
       
         
         string servidor = "smtp.gmail.com";
         int puerto = 587;
          
         string GmailUser = "asupempresas@gmail.com";
         string GmailPass = "revels321";

         string receptor = email;

         MimeMessage message = new ();
         message.From.Add(new MailboxAddress("Compra realizada", GmailUser));
         message.To.Add(new MailboxAddress( receptor , receptor));
         message.Subject = "Confirmacion de compra";

         BodyBuilder cuerpo = new ();
         cuerpo.TextBody = "Gracias por confiar en nosotros";
         cuerpo.HtmlBody = html;
          
         message.Body = cuerpo.ToMessageBody();

         SmtpClient cliente = new ();
         cliente.CheckCertificateRevocation = false;
         cliente.Connect(servidor, puerto, MailKit.Security.SecureSocketOptions.StartTls);
         cliente.Authenticate(GmailUser, GmailPass);
         cliente.Send(message);
         cliente.Disconnect(true);


      }

        public IActionResult DownLoadBoleta(int id){
        
           var compra = _context.DataCompras.Find(id);
           return File(compra.boleta, "application/pdf", "boleta.pdf");  
          
       }
      
       [Authorize(Roles = "profesor,alumno")]
       public IActionResult Transferencia(){
              
            var user = _userManager.GetUserAsync(User);
            var proformas = _context.DataProformas.Include(p => p.curso).Where(p => p.usuario.Id == user.Result.Id && p.Status.Equals("Pendiente")).ToList();

            decimal sueldo = 0;

            if(proformas.Count > 0)
            {
                foreach (var proforma in proformas)
                {
                    sueldo += proforma.curso.precio;
                }
            }
            
            ViewBag.proformas = proformas;
            ViewBag.monto = sueldo;

            ViewData["Title"] = "Pago por transferencia";
            return View();
          
       }

        [Authorize(Roles = "administrador")]
        public IActionResult Pendientes(){
           
           var pagos = _context.DataPagos.Include(p => p.usuario).Where(p => p.Status.Equals("Pendiente")).ToList();
           return View(pagos);
        }
        
        [HttpGet]
        public async Task<ActionResult<String>> getVoucher(int id){

            var pagos =  await _context.DataPagos.FindAsync(id);
            var voucherBase = pagos.fileVoucher;
            return new JsonResult(new { voucher = voucherBase });

        }

        [HttpGet]
        public async Task<ActionResult<String>> desaprobarPago(int id){

            var pagos =  await _context.DataPagos.Include(p => p.usuario).Where(p => p.Id == id).FirstOrDefaultAsync();
            var proformas = _context.DataProformas.Include(p => p.curso).Where(p => p.usuario.Id == pagos.usuario.Id && p.Status.Equals("Validar")).ToList();
            pagos.Status = "Desaprobado";
            _context.Update(pagos);

            foreach (var proforma in proformas)
            {
                proforma.Status = "Pendiente";
            }
           
            _context.UpdateRange(proformas);

           /* string servidor = "smtp.gmail.com";
            int puerto = 587;
            
            string GmailUser = "asupempresas@gmail.com";
            string GmailPass = "revels321";

            string receptor = pagos.usuario.Email;

            MimeMessage message = new ();
            message.From.Add(new MailboxAddress("Compra cancelada", GmailUser));
            message.To.Add(new MailboxAddress( receptor , receptor));
            message.Subject = "Cancelacion de compra";

            BodyBuilder cuerpo = new ();
            cuerpo.TextBody = "El pedidio fue cancelado";
            cuerpo.HtmlBody = " <p>Tu pedido fue cancelado porque la imagen del voucher es una foto no valida o no esta totalmente clara</p>" +
                              " <p>Puedes generar otra vez tu pedido ya que los cursos que seleccionaste vuelven a estar en tu carrito de compras o puedes comunicar al correo de juan@asup.com para mas informacion</p>";
            
            message.Body = cuerpo.ToMessageBody();

            SmtpClient cliente = new ();
            cliente.CheckCertificateRevocation = false;
            cliente.Connect(servidor, puerto, MailKit.Security.SecureSocketOptions.StartTls);
            cliente.Authenticate(GmailUser, GmailPass);
            cliente.Send(message);
            cliente.Disconnect(true);*/

             await _context.SaveChangesAsync();

            return new JsonResult(new { status = "ok" });

        }

        [HttpGet]
        public async Task<ActionResult<String>> aprobarPago(int id){

            var pagos =  await _context.DataPagos.Include(p => p.usuario).Where(p => p.Id == id).FirstOrDefaultAsync();
            var proformas = _context.DataProformas.Include(p => p.curso).Where(p => p.usuario.Id == pagos.usuario.Id && p.Status.Equals("Validar")).ToList();
            var montototal = proformas.Sum(c => c.curso.precio);

            pagos.Status = "Realizado";
            _context.Update(pagos);

            var boleta = GeneratePdfReport(proformas);

            Compra compra = new Compra();
            compra.usuario = _context.DataUsuarios.Find(pagos.usuario.Id);
            compra.Total = pagos.monto;
            compra.Pago = pagos;
            compra.boleta = boleta;
            _context.Add(compra);

            List<DetalleCompra> detalles = new List<DetalleCompra>();
            List<CursoAlumno> cursoAlumnos = new List<CursoAlumno>();
                   
            foreach (var proforma in proformas)
            {
                DetalleCompra detalle = new DetalleCompra();
                detalle.curso = _context.DataCursos.FirstOrDefault(c => c.Id == proforma.curso.Id);
                detalle.compra = compra;
                detalle.Precio = _context.DataCursos.FirstOrDefault(c => c.Id == proforma.curso.Id).precio;
                          

                CursoAlumno cursoAlumno = new CursoAlumno();
                cursoAlumno.Curso = _context.DataCursos.FirstOrDefault(c => c.Id == proforma.curso.Id);
                cursoAlumno.usuario = pagos.usuario;
                cursoAlumno.fechaMatricula = DateTime.Now;


                proforma.Status = "Realizado";

                var curso = _context.DataCursos.FirstOrDefault(c => c.Id == proforma.curso.Id);
                curso.cupo = curso.cupo - 1;
                           
                _context.Update(curso);
                          

                detalles.Add(detalle);
                cursoAlumnos.Add(cursoAlumno);
         }

                _context.AddRange(detalles);
                _context.AddRange(cursoAlumnos);
                _context.UpdateRange(proformas);

                _context.SaveChanges();

                return new JsonResult(new { mensaje = "Pago aprobado" });
        }

       [HttpPost]
       public async Task<ActionResult<String>> Transferencias([FromForm] IFormFile file , [FromForm] string metodo , [FromForm] string titular){
        
             var user = _userManager.GetUserAsync(User);
             var proformas = _context.DataProformas.Include(p => p.curso).Where(p => p.usuario.Id == user.Result.Id && p.Status.Equals("Pendiente")).ToList();
             var montototal = proformas.Sum(c => c.curso.precio);

             Stream str = file.OpenReadStream();
             BinaryReader br = new BinaryReader(str);
             Byte [] fileDet = br.ReadBytes((Int32) str.Length);
              
             Pago pago = new Pago();
             pago.NombreTarjeta = titular;
             pago.Status = "Pendiente";                   
             pago.usuario = user.Result;
             pago.fechaPago = DateTime.Now;
             pago.modalidad = metodo;
             pago.monto = montototal; 
             pago.nombreArchivo = Path.GetFileName(file.FileName);
             pago.fileVoucher = Convert.ToBase64String(fileDet); 

             _context.Add(pago);

             foreach (var proforma in proformas)
             {
                 proforma.Status = "Validar";
             }          
            
            _context.UpdateRange(proformas);

            await _context.SaveChangesAsync();
            
            return new JsonResult(new { mensaje = "Pago creado con exito" });
       }

    }
}