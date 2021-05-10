using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

using Microsoft.Extensions.Logging;

namespace Solarponics.Data
{
    public sealed class DapperStoredProcedure : IStoredProcedure
    {
        private readonly IDbConnection connection;
        private readonly object executeLock = new object();
        private readonly string name;
        private readonly IReadOnlyList<IDbDataParameter> parameters;
        private readonly IDbTransaction transaction;
        private readonly ILogger<DapperStoredProcedure> logger;
        private readonly IDbAdapter dbAdapter;
        private bool disposed;
        private bool executeCalled;
        private SqlMapper.GridReader reader;

        public DapperStoredProcedure(string name, IReadOnlyList<IDbDataParameter> parameters, IDbConnection connection, IDbTransaction transaction, ILogger<DapperStoredProcedure> logger, IDbAdapter dbAdapter)
        {
            this.name = name;
            this.parameters = parameters;
            this.connection = connection;
            this.transaction = transaction;
            this.logger = logger;
            this.dbAdapter = dbAdapter;
        }

        ~DapperStoredProcedure()
        {
            this.Dispose(false);
        }

        private void GuardAgainstInvocationWhenDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(nameof(DapperStoredProcedure));
            }
        }

        private void GuardAgainstMultipleExecutions()
        {
            lock (this.executeLock)
            {
                if (this.executeCalled)
                {
                    throw new StoredProcedureExecutedTooManyTimesException();
                }

                this.executeCalled = true;
            }
        }
        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.reader?.Dispose();
            }

            this.disposed = true;
        }

        public void ExecuteNonQuery()
        {
            this.GuardAgainstInvocationWhenDisposed();
            this.GuardAgainstMultipleExecutions();

            var dynamicParameters = this.GetDynamicParameters();
            this.logger.LogInformation("Calling stored procedure {storedProcedure} (non-query)", this.name);
            this.dbAdapter.Execute(this.connection, this.name, dynamicParameters, this.transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task ExecuteNonQueryAsync()
        {
            this.GuardAgainstInvocationWhenDisposed();
            this.GuardAgainstMultipleExecutions();

            var dynamicParameters = this.GetDynamicParameters();
            this.logger.LogInformation("Calling stored procedure {storedProcedure} (non-query)", this.name);
            await this.dbAdapter.ExecuteAsync(this.connection, this.name, dynamicParameters, this.transaction, commandType: CommandType.StoredProcedure);
        }

        public void ExecuteReader()
        {
            this.GuardAgainstInvocationWhenDisposed();
            this.GuardAgainstMultipleExecutions();
            var dynamicParameters = this.GetDynamicParameters();

            this.logger.LogInformation("Calling stored procedure {storedProcedure} (dataset)", this.name);
            this.reader = this.dbAdapter.QueryMultiple(
                this.connection,
                this.name,
                dynamicParameters,
                commandType: CommandType.StoredProcedure,
                transaction: this.transaction);
        }

        public async Task ExecuteReaderAsync()
        {
            this.GuardAgainstInvocationWhenDisposed();
            this.GuardAgainstMultipleExecutions();
            var dynamicParameters = this.GetDynamicParameters();

            this.logger.LogInformation("Calling stored procedure {storedProcedure} (dataset)", this.name);
            this.reader = await this.dbAdapter.QueryMultipleAsync(
                this.connection,
                this.name,
                dynamicParameters,
                commandType: CommandType.StoredProcedure,
                transaction: this.transaction);
        }

        public async Task<T> ExecuteScalarAsync<T>()
        {
            this.GuardAgainstInvocationWhenDisposed();
            this.GuardAgainstMultipleExecutions();

            var dynamicParameters = this.GetDynamicParameters();
            this.logger.LogInformation("Calling stored procedure {storedProcedure} (scalar)", this.name);
            return (T) await this.dbAdapter.ExecuteScalarAsync<T>(this.connection, this.name, dynamicParameters, this.transaction, commandType: CommandType.StoredProcedure);
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<T>> GetDataSetAsync<T>()
        {
            this.GuardAgainstInvocationWhenDisposed();
            return await this.reader.ReadAsync<T>();
        }

        [ExcludeFromCodeCoverage]
        public async Task<T> GetFirstOrDefaultRowAsync<T>()
        {
            return await this.GetDataSetAsync<T>().ContinueWith(
                response => response.Result.FirstOrDefault());
        }

        private DynamicParameters GetDynamicParameters()
        {
            var dynamicParameters = new DynamicParameters();
            if (this.parameters != null)
            {
                foreach (var param in this.parameters)
                {
                    dynamicParameters.Add(param.ParameterName, param.Value, param.DbType, param.Direction, param.Size, param.Precision, param.Scale);
                }
            }

            return dynamicParameters;
        }
    }
}