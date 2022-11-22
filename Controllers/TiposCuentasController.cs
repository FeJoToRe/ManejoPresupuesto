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
        private readonly IServicioUsuarios userService;

        public TiposCuentasController(IRepositoryTiposCuentas repoTiposCuentas,
            IServicioUsuarios userService)
        {
            this.repoTiposCuentas = repoTiposCuentas;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = userService.GetUserID();
            var tiposCuentas = await repoTiposCuentas.GetByID(usuarioId);
            return View(tiposCuentas);


        }

        public IActionResult Crear()
        {

            return View();

        }
        [HttpGet]
        public async Task<ActionResult> Editar(int ID)
        {

            var usuarioId = userService.GetUserID();
            var tipoCuenta = await repoTiposCuentas.GetByID(ID, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(tipoCuenta);
        }


        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuentaModel tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = userService.GetUserID();

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
            var usuarioId = userService.GetUserID();
            var AlreadyExistsTipoCuenta = await repoTiposCuentas.Existe(nombre, usuarioId);

            if (AlreadyExistsTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);


        }

        [HttpPost]
        public async Task<ActionResult> Editar(TipoCuentaModel tipoCuenta)
        {
            var usuarioId = userService.GetUserID();
            var tipoCuentaExists = await repoTiposCuentas.GetByID(tipoCuenta.ID, usuarioId);

                if (tipoCuentaExists is null)
                {
                return RedirectToAction("NotFound", "Home");
                }
           await repoTiposCuentas.Update(tipoCuenta);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrar(int id)
        {

            var usuarioId = userService.GetUserID();
            var tipoCuenta = await repoTiposCuentas.GetByID(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(tipoCuenta);
        }

        [HttpPost]

        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = userService.GetUserID();
            var tipoCuenta = await repoTiposCuentas.GetByID(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await repoTiposCuentas.Borrar(id);

            return RedirectToAction("Index");
        }
    }
}
