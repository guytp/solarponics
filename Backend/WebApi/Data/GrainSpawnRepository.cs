using System;
using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class GrainSpawnRepository : IGrainSpawnRepository
    {
        private const string ProcedureNameGet = "[dbo].[GrainSpawnGet]";
        private const string ProcedureNameInnoculate = "[dbo].[GrainSpawnInnoculate]";
        private const string ProcedureNamePlaceShelf = "[dbo].[GrainSpawnShelfPlace]";
        private const string ProcedureNameAdd = "[dbo].[GrainSpawnAdd]";

        public GrainSpawnRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<GrainSpawn> Get(int id)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", id)
                });
            await storedProcedure.ExecuteReaderAsync();
            return await storedProcedure.GetFirstOrDefaultRowAsync<GrainSpawn>();
        }
        public async Task<GrainSpawn[]> GetAll()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, null);
            await storedProcedure.ExecuteReaderAsync();
            return (await storedProcedure.GetDataSetAsync<GrainSpawn>()).ToArray();
        }

        public async Task Innoculate(int id, int cultureId, string additionalNotes, int userId, DateTime date)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameInnoculate, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", id),
                    new StoredProcedureParameter("@cultureId", cultureId),
                    new StoredProcedureParameter("@additionalNotes", additionalNotes),
                    new StoredProcedureParameter("@date", date),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task ShelfPlace(int id, int shelfId, string additionalNotes, int userId, DateTime date)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNamePlaceShelf, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", id),
                    new StoredProcedureParameter("@shelfId", shelfId),
                    new StoredProcedureParameter("@additionalNotes", additionalNotes),
                    new StoredProcedureParameter("@date", date),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task<int> Add(int userId, int recipeId, decimal weight, string notes, DateTime date)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAdd, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@userId", userId),
                    new StoredProcedureParameter("@recipeId", recipeId),
                    new StoredProcedureParameter("@weight", weight),
                    new StoredProcedureParameter("@date", date),
                    new StoredProcedureParameter("@notes", notes)
                });
            var id = await storedProcedure.ExecuteScalarAsync<int>();
            return id;
        }
    }
}