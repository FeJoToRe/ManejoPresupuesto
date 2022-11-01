using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces
{
    public interface IRepositoryTiposCuentas
    {

        Task Crear(TipoCuentaModel tipoCuenta);

        Task<bool> Existe(string nombre, int usuarioId);

    }
}
