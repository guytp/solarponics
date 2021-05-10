using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Solarponics.Data
{
    public sealed class DatabaseConnection : IDatabaseConnection, IDisposable
    {
        private readonly object databaseLock = new object();

        private bool disposed;
        private readonly IConfiguration configuration;
        private readonly IStoredProcedureFactory storedProcedureFactory;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public DatabaseConnection(IConfiguration configuration,
            IStoredProcedureFactory storedProcedureFactory,
            IDbConnectionFactory dbConnectionFactory)
        {
            this.configuration = configuration;
            this.storedProcedureFactory = storedProcedureFactory;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        ~DatabaseConnection()
        {
            this.Dispose(false);
        }

        public bool IsInTransaction
        {
            get
            {
                lock (this.databaseLock)
                {
                    return this.Transaction != null;
                }
            }
        }

        private IDbConnection Connection { get; set; }

        private IDbTransaction Transaction { get; set; }
        
        public IStoredProcedure CreateStoredProcedure(string name, IReadOnlyList<IDbDataParameter> parameters)
        {
            this.EnsureOpenConnection();
            return this.storedProcedureFactory.Create(name, parameters, this.Connection, this.Transaction);
        }

        public async Task ExecuteQueryAsync(string queryText, CommandType commandType)
        {
            this.EnsureOpenConnection();
            await this.Connection.ExecuteAsync(queryText, null, this.Transaction, null, commandType);
        }

        public void EnsureOpenConnection()
        {
            lock (this.databaseLock)
            {
                this.GuardAgainstInvocationWhenDisposed();

                if (this.Connection == null)
                {
                    this.Connection = this.dbConnectionFactory.Create();
                    this.Connection.ConnectionString = this.configuration.GetConnectionString("DefaultConnection");
                }

                if (this.Connection.State != ConnectionState.Open)
                {
                    this.Connection.Open();
                }

                if (this.Connection.State != ConnectionState.Open)
                {
                    throw new DatabaseConnectionUnavailableException();
                }
            }
        }

        public void TransactionBegin()
        {
            lock (this.databaseLock)
            {
                this.GuardAgainstInvocationWhenDisposed();

                this.EnsureOpenConnection();

                if (this.IsInTransaction)
                {
                    throw new DatabaseTransactionAlreadyInProgressException();
                }

                this.Transaction = this.Connection.BeginTransaction();
            }
        }

        public void TransactionCommit()
        {
            lock (this.databaseLock)
            {
                this.GuardAgainstInvocationWhenDisposed();

                this.EnsureOpenConnection();

                if (!this.IsInTransaction)
                {
                    throw new DatabaseTransactionNotInProgressException();
                }

                this.Transaction.Commit();
                this.Transaction = null;
            }
        }

        public void TransactionRollback()
        {
            lock (this.databaseLock)
            {
                this.GuardAgainstInvocationWhenDisposed();

                this.EnsureOpenConnection();

                if (!this.IsInTransaction)
                {
                    throw new DatabaseTransactionNotInProgressException();
                }

                this.Transaction.Rollback();
                this.Transaction = null;
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
                if (this.IsInTransaction)
                {
                    this.TransactionRollback();
                }

                if (this.Connection != null)
                {
                    this.Connection.Dispose();
                    this.Connection = null;
                }
            }

            this.disposed = true;
        }

        private void GuardAgainstInvocationWhenDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(nameof(DatabaseConnection));
            }
        }
    }
}