using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController: Controller
    {

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(TiposCuentasController tiposCuentas)
        {
            if (!ModelState.IsValid) {
                return View(tiposCuentas);
            }

            return View();

        }

    }
}
