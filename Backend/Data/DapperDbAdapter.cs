using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dapper;

namespace Solarponics.Data
{
    [ExcludeFromCodeCoverage]
    public class DapperDbAdapter : IDbAdapter
    {
        public void Execute(IDbConnection connection, string name, DynamicParameters dynamicParameters,
            IDbTransaction transaction, CommandType commandType)
        {
            connection.Execute(name, dynamicParameters, transaction, commandType: commandType);
        }

        public async Task ExecuteAsync(IDbConnection connection, string name, DynamicParameters dynamicParameters,
            IDbTransaction transaction, CommandType commandType)
        {
            await connection.ExecuteAsync(name, dynamicParameters, transaction, commandType: commandType);
        }

        public SqlMapper.GridReader QueryMultiple(IDbConnection connection, string name,
            DynamicParameters dynamicParameters, CommandType commandType, IDbTransaction transaction)
        {
            return connection.QueryMultiple(name, dynamicParameters, commandType: commandType,
                transaction: transaction);
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(IDbConnection connection, string name,
            DynamicParameters dynamicParameters, CommandType commandType, IDbTransaction transaction)
        {
            return await connection.QueryMultipleAsync(name, dynamicParameters, commandType: commandType,
                transaction: transaction);
        }

        public async Task<T> ExecuteScalarAsync<T>(IDbConnection connection, string name,
            DynamicParameters dynamicParameters, IDbTransaction transaction, CommandType commandType)
        {
            return await connection.ExecuteScalarAsync<T>(name, dynamicParameters, transaction,
                commandType: commandType);
        }
    }
}