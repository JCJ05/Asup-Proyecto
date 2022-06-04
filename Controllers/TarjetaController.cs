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

namespace Repaso_Net.Controllers
{
    public class TarjetaController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<TarjetaController> _logger;
        public TarjetaController(ApplicationDbContext context, ILogger<TarjetaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
    }
}