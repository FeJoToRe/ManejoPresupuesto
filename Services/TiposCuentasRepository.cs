using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services
{
    /*Un Repository es una clase que accederá a los datos en la base de datos*/

    public class TiposCuentasRepository : IRepositoryTiposCuentas
    {
        private readonly string connString;

        public TiposCuentasRepository(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuentaModel tipoCuenta)
        {
            using var conn = new SqlConnection(connString);

            var id = await conn.QuerySingleAsync<int>($@"insert into TiposCuentas(NombreCuenta, UsuarioId, Orden)
                                            values (@Nombre, @UsuarioId, 0);
                                            select scope_identity();
                                            ", tipoCuenta);
            tipoCuenta.ID = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {

            using var conn = new SqlConnection(connString);
            var exists = await conn.QueryFirstOrDefaultAsync<int>($@"select 1 from TiposCuentas
                                                                     where NombreCuenta = @Nombre and 
                                                                     UsuarioId = @UsuarioId", new{nombre, usuarioId});
            return exists == 1;
        
        }

        public async Task<IEnumerable<TipoCuentaModel>> GetByID(int usuarioId)
        {
            using var connection = new SqlConnection(connString);
            return await connection.QueryAsync<TipoCuentaModel>(@"select TipoCuentaId, NombreCuenta, Orden
                                                                 from TiposCuentas
                                                                 where UsuarioId = @usuarioId", new {usuarioId});

        }

        public async Task Update(TipoCuentaModel tipoCuenta)
        {
            using var connection = new SqlConnection(connString);
            await connection.ExecuteAsync(@"update TiposCuentas
                                             set NombreCuenta = @Nombre
                                             where TipoCuentaId = @ID", tipoCuenta);

        }

        public async Task<TipoCuentaModel> GetByID(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuentaModel>
                (@"select TipoCuentaId, NombreCuenta, Orden
                    from TiposCuentas
                    where TipoCuentaId = @id and UsuarioId = @usuarioId", new {id, usuarioId});

        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connString);

            await connection.ExecuteAsync("delete TiposCuentas where TipoCuentaId = @id", new {id});


        }


    }
}
