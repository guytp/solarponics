using System.Collections.Generic;
using System.Data;

namespace Solarponics.Data
{
    public interface IStoredProcedureFactory
    {
        IStoredProcedure Create(string name, IReadOnlyList<IDbDataParameter> parameters, IDbConnection connection, IDbTransaction transaction);
    }
}