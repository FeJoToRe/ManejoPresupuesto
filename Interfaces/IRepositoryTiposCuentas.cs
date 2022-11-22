using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces
{
    public interface IRepositoryTiposCuentas
    {
        Task Borrar(int id);
        Task Crear(TipoCuentaModel tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<TipoCuentaModel> GetByID(int id, int usuarioId);
        Task<IEnumerable<TipoCuentaModel>> GetByID(int usuarioId);
        Task Update(TipoCuentaModel tipoCuenta);
    }
}
