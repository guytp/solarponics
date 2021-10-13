using System;
using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class RoomRepository : IRoomRepository
    {
        private const string ProcedureNameGet = "[dbo].[RoomGet]";
        private const string ProcedureNameAdd = "[dbo].[RoomAdd]";

        public RoomRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<Room[]> Get()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, null);
            await storedProcedure.ExecuteReaderAsync();
            return (await storedProcedure.GetDataSetAsync<Room>()).ToArray();
        }

        public async Task<int> Add(int locationId, string name, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAdd, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@locationId", locationId),
                    new StoredProcedureParameter("@name", name),
                    new StoredProcedureParameter("@userId", userId)
                });
            return await storedProcedure.ExecuteScalarAsync<int>();
        }
    }
}