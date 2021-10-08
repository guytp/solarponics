using System;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class CultureRepository : ICultureRepository
    {
        private const string ProcedureNameGet = "[dbo].[CultureGet]";
        private const string ProcedureNameInnoculate = "[dbo].[CultureGet]";
        private const string ProcedureNameAdd = "[dbo].[CultureAdd]";

        public CultureRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<Culture> Get(int id)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", id)
                });
            await storedProcedure.ExecuteReaderAsync();
            return await storedProcedure.GetFirstOrDefaultRowAsync<Culture>();
        }

        public async Task Innoculate(int id, int parentCultureId, string additionalNotes, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameInnoculate, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", id),
                    new StoredProcedureParameter("@parentCultureId", parentCultureId),
                    new StoredProcedureParameter("@additionalNotes", additionalNotes),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task<int> Add(int? supplierId, int? parentCultureId, int userId, int? recipeId, CultureMediumType mediumType, DateTime? orderDate, string strain, string notes)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAdd, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@supplierId", supplierId),
                    new StoredProcedureParameter("@parentCultureId", parentCultureId),
                    new StoredProcedureParameter("@userId", userId),
                    new StoredProcedureParameter("@recipeId", recipeId),
                    new StoredProcedureParameter("@mediumType", (int)mediumType),
                    new StoredProcedureParameter("@orderDate", orderDate),
                    new StoredProcedureParameter("@strain", strain),
                    new StoredProcedureParameter("@notes", notes)
                });
            var id = await storedProcedure.ExecuteScalarAsync<int>();
            return id;
        }
    }
}