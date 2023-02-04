using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces
{
    public interface IRepositorioCuentas
    {
        Task Crear(Cuenta cuenta);
    }
}
