using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

namespace Solarponics.Data
{
    [ExcludeFromCodeCoverage]
    public sealed class DapperStoredProcedureFactory : IStoredProcedureFactory
    {
        private readonly IServiceProvider serviceProvider;

        public DapperStoredProcedureFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IStoredProcedure Create(string name, IReadOnlyList<IDbDataParameter> parameters, IDbConnection connection, IDbTransaction transaction)
        {
            var logger = (ILogger<DapperStoredProcedure>) this.serviceProvider.GetService(typeof(ILogger<DapperStoredProcedure>));
            return new DapperStoredProcedure(name, parameters, connection, transaction, logger, new DapperDbAdapter());
        }
    }
}