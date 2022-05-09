using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Repaso_Net.Models;

namespace Repaso_Net.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<Usuario> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }

                        string servidor = "smtp.gmail.com";
                        int puerto = 587;

                        string GmailUser = "soyelputoamocabron23@gmail.com";
                        string GmailPass = "revels321";

                        string receptor = email;

                        MimeMessage message = new ();
                        message.From.Add(new MailboxAddress("Verificacion de correo", GmailUser));
                        message.To.Add(new MailboxAddress(email , email));
                        message.Subject = "Confirmacion de cuenta";

                        BodyBuilder cuerpo = new ();
                        cuerpo.TextBody = "Hola como estas";
                        cuerpo.HtmlBody = " <p>Para confirmar tu cuenta, por favor haz click en el siguiente enlace:</p>" +
                            "<p><a href=\"" + EmailConfirmationUrl + "\">Confirmar cuenta</a></p>";
                           
                        

                        message.Body = cuerpo.ToMessageBody();

                        SmtpClient cliente = new ();
                        cliente.CheckCertificateRevocation = false;
                        cliente.Connect(servidor, puerto, MailKit.Security.SecureSocketOptions.StartTls);
                        cliente.Authenticate(GmailUser, GmailPass);
                        cliente.Send(message);
                        cliente.Disconnect(true);

         
                       ViewData["email"] = email;
                       return Page();
        }
       
        
      
}

    
    }
