using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Solarponics.Data
{
    public interface IDatabaseConnection
    {
        IStoredProcedure CreateStoredProcedure(string name, IReadOnlyList<IDbDataParameter> parameters);

        Task ExecuteQueryAsync(string queryText, CommandType commandType);

        void TransactionBegin();

        void TransactionCommit();

        void TransactionRollback();
    }
}