using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Repaso_Net.Data;
using Repaso_Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Repaso_Net.Controllers.Rest
{
    [ApiController]
    [Route("api/tarjeta")]
    public class TarjetaController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public TarjetaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<String>> validarTarjeta(string nombreTarjeta, string numeroTarjeta, string duedate, string cvv, string nombreTitular, decimal monto){

            string mensaje = "";

            if(existeNumeroTarjeta(numeroTarjeta)){
                
                var flag = 1; //Datos de la tarjeta correctos

                var tarjeta = obtenerTarjetaPorNumero(numeroTarjeta);

                if(!validarNombreTarjeta(numeroTarjeta, nombreTarjeta)){
                    flag = 0; //Datos de la tarjeta incorrectos
                    mensaje += "El banco de la tarjeta no es correcto.\n";
                }

                if(!validarDueDate(numeroTarjeta, duedate)){
                    flag = 0;
                    mensaje += "La fecha de vencimiento de la tarjeta no es correcta.\n";
                }

                if(!validarCvv(numeroTarjeta, cvv)){
                    flag = 0;
                    mensaje += "El CVV de la tarjeta no es correcto.\n";
                }

                if(!validarNombreTitular(numeroTarjeta, nombreTitular)){
                    flag = 0;
                    mensaje += "El nombre del titular de la tarjeta no es correcto.\n";
                }

                if(flag == 1){
                    if(!validarSaldo(numeroTarjeta, monto)){
                        mensaje = "La tarjeta no tiene saldo suficiente para hacer el pago.";
                    }else{
                        mensaje = "Los datos de la tarjeta son correctos.";
                    }
                }

            }else{
                mensaje = "No existe ninguna tarjeta registrada con ese número. Por favor, vuelva a intentarlo.";
            }

            return new JsonResult(new { mensajeTarjeta = mensaje });

        }

        public Tarjeta obtenerTarjetaPorNumero(string numeroTarjeta){
            
            var tarjetas = _context.DataTarjetas.ToList();
            
            if(existeNumeroTarjeta(numeroTarjeta)){
                var tarjeta = tarjetas.FirstOrDefault(x => x.NumeroTarjeta.Equals(numeroTarjeta));
                return tarjeta;
            }else{
                return null;
            }

        }
        
        public Boolean existeNumeroTarjeta(string numeroTarjeta){

            var tarjetas = _context.DataTarjetas.ToList();

            var tarjeta = tarjetas.FirstOrDefault(x => x.NumeroTarjeta.Equals(numeroTarjeta));

            if(tarjeta != null){
                return true;
            }else{
                return false;
            }

        }

        public Boolean validarNombreTarjeta(string numeroTarjeta, string nombreTarjeta){

            var tarjeta = obtenerTarjetaPorNumero(numeroTarjeta);

            if(tarjeta.NombreTarjeta.Equals(nombreTarjeta)){
                return true;
            }else{
                return false;
            }

        }

        public Boolean validarDueDate(string numeroTarjeta, string dueDate){

            var tarjeta = obtenerTarjetaPorNumero(numeroTarjeta);

            if(tarjeta.DueDateYYMM.Equals(dueDate)){
                return true;
            }else{
                return false;
            }

        }

        public Boolean validarCvv(string numeroTarjeta, string cvv){

            var tarjeta = obtenerTarjetaPorNumero(numeroTarjeta);

            if(tarjeta.Cvv.Equals(cvv)){
                return true;
            }else{
                return false;
            }

        }

        public Boolean validarNombreTitular(string numeroTarjeta, string nombreTitular){

            var tarjeta = obtenerTarjetaPorNumero(numeroTarjeta);

            if(tarjeta.NombreTitular.Equals(nombreTitular)){
                return true;
            }else{
                return false;
            }

        }

        public Boolean validarSaldo(string numeroTarjeta, decimal monto){

            var tarjeta = obtenerTarjetaPorNumero(numeroTarjeta);

            if(tarjeta.Saldo >= monto){
                return true;
            }else{
                return false;
            }

        }

        [HttpPost]
        public async Task<ActionResult<String>> ActualizarSaldo(string numeroTarjeta, decimal monto){

            var tarjeta = obtenerTarjetaPorNumero(numeroTarjeta);

            string mensaje = "";

            if(validarSaldo(numeroTarjeta, monto)){
                tarjeta.Saldo = tarjeta.Saldo - monto;
                _context.Update(tarjeta);
                await _context.SaveChangesAsync();
                mensaje = "Se actualizó el saldo de la tarjeta";
            }else{
                mensaje = "No se actualizó nada";
            }

            return new JsonResult(new { mensajeSaldo = mensaje });

        }

    }
}