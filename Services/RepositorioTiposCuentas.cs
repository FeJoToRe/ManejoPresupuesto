using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Services
{
    /*Un Repository es una clase que accederá a los datos en la base de datos*/

    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionString;

        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var conn = new SqlConnection(connectionString);

            var id = await conn.QuerySingleAsync<int>($"TiposCuentasInsertar", new {usuarioId = tipoCuenta.UsuarioId,
                                                                                    nombre = tipoCuenta.Nombre}, 
                                                                                    commandType: System.Data.CommandType.StoredProcedure);
            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {

            using var conn = new SqlConnection(connectionString);
            var exists = await conn.QueryFirstOrDefaultAsync<int>($@"select 1 from TiposCuentas
                                                                     where Nombre = @Nombre and 
                                                                     UsuarioId = @UsuarioId", new{nombre, usuarioId});
            return exists == 1;
        
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"select Id, Nombre, Orden
                                                                 from TiposCuentas
                                                                 where UsuarioId = @usuarioId
                                                                 ORDER BY Orden", new {usuarioId});

        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update TiposCuentas
                                             set Nombre = @Nombre
                                             where Id = @Id", tipoCuenta);

        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>
                (@"select Id, Nombre, Orden
                    from TiposCuentas
                    where Id = @Id and UsuarioId = @usuarioId", new {id, usuarioId});

        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("delete TiposCuentas where Id = @Id", new {id});


        }

        public async Task Ordenar(IEnumerable<TipoCuenta> tiposCuentasOrdenados)
        {
            var query = "UPDATE TiposCuentas SET Orden = @Orden where Id = @Id;";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, tiposCuentasOrdenados);

        } 
    }
}
