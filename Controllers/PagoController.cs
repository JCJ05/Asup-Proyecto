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

namespace Asup_Proyecto.Controllers
{
    public class PagoController : Controller
    {

        private readonly ApplicationDbContext _context;

        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<PagoController> _logger;

        public PagoController(ILogger<PagoController> logger, ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Transferencia()
        {
            Pago pago = new Pago();
            return View(pago);
        }

        [HttpPost]
        public IActionResult Transferencia(Pago pago)
        {
            ViewData["Mensaje"] = "Muchas gracias por realizar el pago, recibirá una confirmación cuando se procese.";
            return View("Confirmacion");
        }

    }
}