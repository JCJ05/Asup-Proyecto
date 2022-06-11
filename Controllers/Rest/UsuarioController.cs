
using System.Collections.Generic;
using System.Threading.Tasks;
using Repaso_Net.Data;
using Repaso_Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Repaso_Net.Controllers.Rest
{   

    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
       
        private readonly UserManager<Usuario> _userManager;
        private readonly ApplicationDbContext _context;
        public UsuarioController(ApplicationDbContext context , UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPut]
        public async Task<ActionResult<String>> validateEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
               return new JsonResult(new { mensaje = "Email no encontrado" });
            }
            else
            {
              return new JsonResult(new { mensaje = "Email Encontrado" });
            }
        }

        [HttpDelete]
        public async Task<ActionResult<String>> validateDni(string dni)
        {     

                var usuario =  _userManager.GetUserAsync(User).Result;
                 
                if(usuario != null && usuario.identificacion == dni){
                    
                    return new JsonResult(new { mensaje = "DNI no encontrado" });
                    
                }else{

                    var user = await  _context.DataUsuarios.Where(x => x.identificacion == dni).FirstOrDefaultAsync();

                    if (user == null)
                    {
                        return new JsonResult(new { mensaje = "DNI no encontrado" });
                    }
                    else
                    {
                        return new JsonResult(new { mensaje = "DNI encontrado" });
                    }

                }

        }
     
          [HttpGet]
        public async Task<ActionResult<string>> GetTodoItems(string email , string url = null , string code = null , string returnUrl = null)
        {
                        
                        var user = await _userManager.FindByEmailAsync(email);

                        if (user == null)
                        {
                            return NotFound($"No existe el usuario con el correo:  '{email}'.");
                        }
                        
                        string urlFinal = url + "&code=" + code + "&returnUrl=" + returnUrl;

                        string servidor = "smtp.gmail.com";
                        int puerto = 587;

                        string GmailUser = "asupempresas@gmail.com";
                        string GmailPass = "revels321";

                        string receptor = email;

                        MimeMessage message = new ();
                        message.From.Add(new MailboxAddress("Verificacion de correo", GmailUser));
                        message.To.Add(new MailboxAddress(email , email));
                        message.Subject = "Confirmacion de cuenta";

                        BodyBuilder cuerpo = new ();
                        cuerpo.TextBody = "Hola como estas";
                        cuerpo.HtmlBody = " <p>Para confirmar tu cuenta, por favor haz click en el siguiente enlace:</p>" +
                            "<p><a href=\"" + urlFinal + "\">Confirmar cuenta</a></p>";
                           
                        

                        message.Body = cuerpo.ToMessageBody();

                        SmtpClient cliente = new ();
                        cliente.CheckCertificateRevocation = false;
                        cliente.Connect(servidor, puerto, MailKit.Security.SecureSocketOptions.StartTls);
                        cliente.Authenticate(GmailUser, GmailPass);
                        cliente.Send(message);
                        cliente.Disconnect(true);

                        
                        
                        return new JsonResult(new { message = urlFinal });

        }

       
        
    }
}