using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repoTiposCuentas;
        private readonly IServicioUsuarios servicioUsuarios;

        public TiposCuentasController(IRepositorioTiposCuentas repoTiposCuentas,
            IServicioUsuarios userService)
        {
            this.repoTiposCuentas = repoTiposCuentas;
            this.servicioUsuarios = userService;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await repoTiposCuentas.Obtener(usuarioId);
            return View(tiposCuentas);


        }

        public IActionResult Crear()
        {

            return View();

        }
        [HttpGet]
        public async Task<ActionResult> Editar(int ID)
        {

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repoTiposCuentas.ObtenerPorId(ID, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoCuenta);
        }


        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = servicioUsuarios.ObtenerUsuarioId();

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
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            /*validacion en front-end con JS*/
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var AlreadyExistsTipoCuenta = await repoTiposCuentas.Existe(nombre, usuarioId);

            if (AlreadyExistsTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe");
            }

            return Json(true);


        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await repoTiposCuentas.Obtener(usuarioId);
            var idsTiposCuentas = tiposCuentas.Select(x => x.Id);
            var idsTiposCuentasNoPertenecenAlUsuario = ids.Except(idsTiposCuentas).ToList();

            if (idsTiposCuentasNoPertenecenAlUsuario.Count > 0 )
            {
                return Forbid();
            }

            var tiposCuentasOrdenados = ids.Select((valor, indice) => 
            new TipoCuenta() { Id = valor, Orden = indice+1 }).AsEnumerable();

            await repoTiposCuentas.Ordenar(tiposCuentasOrdenados);

            return Ok();

        }

        [HttpPost]
        public async Task<ActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuentaExists = await repoTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);

                if (tipoCuentaExists is null)
                {
                return RedirectToAction("NoEncontrado", "Home");
                }
           await repoTiposCuentas.Actualizar(tipoCuenta);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrar(int id)
        {

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repoTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repoTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repoTiposCuentas.Borrar(id);

            return RedirectToAction("Index");
        }
    }
}
