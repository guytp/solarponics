using System;
using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class FruitingBlockRepository : IFruitingBlockRepository
    {
        private const string ProcedureNameGet = "[dbo].[FruitingBlockGet]";
        private const string ProcedureNameInnoculate = "[dbo].[FruitingBlockInnoculate]";
        private const string ProcedureNamePlaceShelfIncubate = "[dbo].[FruitingBlockIncubateShelfPlace]";
        private const string ProcedureNamePlaceShelfFruiting = "[dbo].[FruitingBlockFruitingShelfPlace]";
        private const string ProcedureNameAdd = "[dbo].[FruitingBlockAdd]";

        public FruitingBlockRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<FruitingBlock> Get(int id)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", id)
                });
            await storedProcedure.ExecuteReaderAsync();
            return await storedProcedure.GetFirstOrDefaultRowAsync<FruitingBlock>();
        }
        public async Task<FruitingBlock[]> GetAll()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, null);
            await storedProcedure.ExecuteReaderAsync();
            return (await storedProcedure.GetDataSetAsync<FruitingBlock>()).ToArray();
        }

        public async Task Innoculate(int id, int cultureId, string additionalNotes, DateTime date, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameInnoculate, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", id),
                    new StoredProcedureParameter("@grainSpawnId", cultureId),
                    new StoredProcedureParameter("@additionalNotes", additionalNotes),
                    new StoredProcedureParameter("@date", date),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task ShelfPlaceIncubate(int id, int shelfId, string additionalNotes, DateTime date, int userId)
        {
            await this.ShelfPlace(id, shelfId, additionalNotes, date, userId, ProcedureNamePlaceShelfIncubate);
        }

        public async Task ShelfPlaceFruiting(int id, int shelfId, string additionalNotes, DateTime date, int userId)
        {
            await this.ShelfPlace(id, shelfId, additionalNotes, date, userId, ProcedureNamePlaceShelfFruiting);
        }

        private async Task ShelfPlace(int id, int shelfId, string additionalNotes, DateTime date, int userId, string procedureName)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(procedureName, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", id),
                    new StoredProcedureParameter("@shelfId", shelfId),
                    new StoredProcedureParameter("@additionalNotes", additionalNotes),
                    new StoredProcedureParameter("@date", date),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task<int> Add(int userId, int recipeId, decimal weight, DateTime date, string notes)
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