using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class AutoclaveRepository : IAutoclaveRepository
    {
        private const string ProcedureNameAdd = "[dbo].[AutoclaveAdd]";
        private const string ProcedureNameDelete = "[dbo].[AutoclaveDelete]";
        private const string ProcedureNameGet = "[dbo].[AutoclaveGet]";

        public AutoclaveRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }
        
        public async Task<int> Add(Autoclave autoclave, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAdd, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@name", autoclave.Name),
                    new StoredProcedureParameter("@details", autoclave.Details),
                    new StoredProcedureParameter("@roomId", autoclave.RoomId),
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

        public async Task<Autoclave[]> Get()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, null);
            await storedProcedure.ExecuteReaderAsync();
            return (await storedProcedure.GetDataSetAsync<Autoclave>()).ToArray();
        }
    }
}