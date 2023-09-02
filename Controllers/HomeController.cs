using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repaso_Net.Models;
using Microsoft.AspNetCore.Identity;

using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.Json;

namespace Repaso_Net.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

    
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
             var apiKey = "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";
            var clientes = new SendGridClient(apiKey);
            var from = new EmailAddress("fmarangon43@gmail.com", "Asup");
            var subject = "Confirmacion de compra";
            var to = new EmailAddress("julio_aguero@usmp.pe" ,  "Julio Aguero");
            var plainTextContent = "Gracias por confiar en nosotros";
            var htmlContent = "<strong>Gracias por confiar en nosotros</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await clientes.SendEmailAsync(msg);

            Console.WriteLine(JsonSerializer.Serialize(response));
            return View();
        }

        public IActionResult Privacy()
        {
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
