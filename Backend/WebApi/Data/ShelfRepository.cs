using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class ShelfRepository : IShelfRepository
    {
        private const string ProcedureNameAdd = "[dbo].[ShelfAdd]";
        private const string ProcedureNameDelete = "[dbo].[ShelfDelete]";
        private const string ProcedureNameGet = "[dbo].[ShelfGet]";

        public ShelfRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }
        
        public async Task<int> Add(Shelf shelf, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAdd, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@name", shelf.Name),
                    new StoredProcedureParameter("@roomId", shelf.RoomId),
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

        public async Task<Shelf[]> Get()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, null);
            await storedProcedure.ExecuteReaderAsync();
            return (await storedProcedure.GetDataSetAsync<Shelf>()).ToArray();
        }
    }
}