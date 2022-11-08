using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositoryTiposCuentas repoTiposCuentas;

        public TiposCuentasController(IRepositoryTiposCuentas repoTiposCuentas)
        {
            this.repoTiposCuentas = repoTiposCuentas;

        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = 1;
            var tiposCuentas = await repoTiposCuentas.Obtener(usuarioId);
            return View(tiposCuentas);


        }

        public IActionResult Crear()
        {

            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuentaModel tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = 1;

            var AlreadyExistsTipoCuenta = await repoTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (AlreadyExistsTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe");

                return View(tipoCuenta);
            }

            await repoTiposCuentas.Crear(tipoCuenta);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> VerifyExistenceTipoCuenta(string nombre)
        {
            /*validacion en front-end con JS*/
            var usuarioId = 1;
            var AlreadyExistsTipoCuenta = await repoTiposCuentas.Existe(nombre, usuarioId);

            if (AlreadyExistsTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);


        }

    }
}
