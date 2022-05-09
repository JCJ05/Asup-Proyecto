using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repaso_Net.Models;

namespace Repaso_Net.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public IndexModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {  

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nombres: ")]
        public string nombres { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Apellidos: ")]
        public string apellidos { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Identificación: ")]
        public string identificacion { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Direccion: ")]
        public string direccion { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(Usuario user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {   
                nombres = user.nombres,
                apellidos = user.apellidos,
                identificacion = user.identificacion,
                direccion = user.direccion,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.nombres != user.nombres)
            {
                user.nombres = Input.nombres;
            }

            if (Input.apellidos != user.apellidos)
            {
                user.apellidos = Input.apellidos;
            }

            if (Input.identificacion != user.identificacion)
            {
                user.identificacion = Input.identificacion;
            }

            if (Input.direccion != user.direccion)
            {
                user.direccion = Input.direccion;
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
