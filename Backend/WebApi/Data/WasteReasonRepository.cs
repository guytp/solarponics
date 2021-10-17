using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class WasteReasonRepository : IWasteReasonRepository
    {
        private const string ProcedureNameAdd = "[dbo].[WasteReasonAdd]";
        private const string ProcedureNameDelete = "[dbo].[WasteReasonDelete]";
        private const string ProcedureNameGet = "[dbo].[WasteReasonGet]";

        public WasteReasonRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }
        
        public async Task<int> Add(WasteReason wasteReason, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAdd, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@reason", wasteReason.Reason),
                    new StoredProcedureParameter("@userId", userId)
                });
            return await storedProcedure.ExecuteScalarAsync<int>();
        }
        
        public async Task Delete(int id, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameDelete, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", id),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task<WasteReason[]> Get()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, null);
            await storedProcedure.ExecuteReaderAsync();
            return (await storedProcedure.GetDataSetAsync<WasteReason>()).ToArray();
        }
    }
}