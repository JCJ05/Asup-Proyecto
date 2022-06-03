using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Repaso_Net.Models;



namespace Repaso_Net.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender ,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {   
        [Required(ErrorMessage ="El campo nombre es requerido")]
        [Display(Name = "Nombres: ")]
        public string nombres { get; set; }

        [Required(ErrorMessage ="El campo apellido es requerido")]
        [Display(Name = "Apellidos: ")]
        public string apellidos { get; set; }

        [Required(ErrorMessage ="El campo identificación es requerido")]
        [DataType(DataType.Text)]
        [MinLength(8, ErrorMessage ="La identificación debe tener al menos 8 caracteres")]
        [Display(Name = "Identificación: ")]
        public string identificacion { get; set; }

        [Required(ErrorMessage ="El campo dirección es requerido")]
        [DataType(DataType.Text)]
        [Display(Name = "Direccion: ")]
        public string direccion { get; set; }

        [Required(ErrorMessage ="El campo correo es requerido")]
        [EmailAddress(ErrorMessage ="El campo correo no tiene un formato valido")]
        [Display(Name = "Correo: ")]
        public string Email { get; set; }

        [Required(ErrorMessage ="El campo contraseña es requerido")]
        [StringLength(100, ErrorMessage = "Las contraseñas deben tener una letra mayuscula otra minuscula y un caracter especial", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña:")]
            public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            var role = await _roleManager.FindByIdAsync("1002");

            if (ModelState.IsValid)
            {
                var user = new Usuario {
                     nombres = Input.nombres,
                     apellidos = Input.apellidos,
                     identificacion = Input.identificacion,
                     direccion = Input.direccion,
                     UserName = Input.Email, 
                     Email = Input.Email 
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, role.Name);
                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {   

                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {  
                    if(error.Code == "DuplicateUserName")
                    {
                        ModelState.AddModelError(string.Empty, "El correo ya esta registrado");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                   
                }
            }

            
            return Page();
        }
    }
}
