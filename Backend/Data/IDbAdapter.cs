using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Solarponics.Data
{
    public interface IDbAdapter
    {
        void Execute(IDbConnection connection, string name, DynamicParameters dynamicParameters,
            IDbTransaction transaction, CommandType commandType);

        Task ExecuteAsync(IDbConnection connection, string name, DynamicParameters dynamicParameters,
            IDbTransaction transaction, CommandType commandType);

        SqlMapper.GridReader QueryMultiple(IDbConnection connection, string name, DynamicParameters dynamicParameters,
            CommandType commandType, IDbTransaction transaction);

        Task<SqlMapper.GridReader> QueryMultipleAsync(IDbConnection connection, string name,
            DynamicParameters dynamicParameters, CommandType commandType, IDbTransaction transaction);

        Task<T> ExecuteScalarAsync<T>(IDbConnection connection, string name, DynamicParameters dynamicParameters,
            IDbTransaction transaction, CommandType commandType);
    }
}