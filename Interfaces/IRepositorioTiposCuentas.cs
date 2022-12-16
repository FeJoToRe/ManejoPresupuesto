using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces
{
    public interface IRepositorioTiposCuentas
    {
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Ordenar(IEnumerable<TipoCuenta> tiposCuentasOrdenados);
    }
}
