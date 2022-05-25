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

            return View(compras);
        }

        public byte[] GeneratePdfReport(List<Proforma> proformas)
    {   

            var total = proformas.Sum(c => c.curso.precio);

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
            sendBoletaToClient(cabecera);

        
        GlobalSettings globalSettings = new GlobalSettings();
        globalSettings.ColorMode = ColorMode.Color;
        globalSettings.Orientation = Orientation.Portrait;
        globalSettings.PaperSize = PaperKind.A4;
        globalSettings.Margins = new MarginSettings { Top = 25, Bottom = 25 };
        ObjectSettings objectSettings = new ObjectSettings();
        objectSettings.PagesCount = true;
        objectSettings.HtmlContent = cabecera;
        WebSettings webSettings = new WebSettings();
        webSettings.DefaultEncoding = "utf-8";
        HeaderSettings headerSettings = new HeaderSettings();
        headerSettings.FontSize = 18;
        headerSettings.FontName = "Ariel";
        headerSettings.Right = "Boleta Electronica";
        headerSettings.Line = true;
        FooterSettings footerSettings = new FooterSettings();
        footerSettings.FontSize = 18;
        footerSettings.FontName = "Ariel";
        footerSettings.Center = "Este recibo sirve para cualquier reclamo";
        footerSettings.Line = true;
        objectSettings.HeaderSettings = headerSettings;
        objectSettings.FooterSettings = footerSettings;
        objectSettings.WebSettings = webSettings;
        HtmlToPdfDocument htmlToPdfDocument = new HtmlToPdfDocument()
        {
            GlobalSettings = globalSettings,
            Objects = { objectSettings },
        };

        return _converter.Convert(htmlToPdfDocument);

      }

      public void sendBoletaToClient(string html){
       
         var usuario = _userManager.GetUserAsync(User);
         
         string servidor = "smtp.gmail.com";
         int puerto = 587;
          
         string GmailUser = "asupempresas@gmail.com";
         string GmailPass = "revels321";

         string receptor = usuario.Result.Email;

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

        public IActionResult DownLoadNormativa(int id){
        
           var compra = _context.DataCompras.Find(id);
           return File(compra.boleta, "application/pdf", "boleta.pdf");  
          
       }

    }
}