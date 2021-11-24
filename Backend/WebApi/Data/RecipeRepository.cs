using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class RecipeRepository : IRecipeRepository
    {
        private const string ProcedureNameAdd = "[dbo].[RecipeAdd]";
        private const string ProcedureNameDelete = "[dbo].[RecipeDelete]";
        private const string ProcedureNameGet = "[dbo].[RecipeGet]";
        private const string ProcedureNameUpdate = "[dbo].[RecipeUpdate]";

        public RecipeRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }
        
        public async Task<int> Add(Recipe recipe, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAdd, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@name", recipe.Name),
                    new StoredProcedureParameter("@type", recipe.Type),
                    new StoredProcedureParameter("@text", recipe.Text),
                    new StoredProcedureParameter("@unitsCreated", recipe.UnitsCreated),
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

        public async Task<Recipe[]> Get()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, null);
            await storedProcedure.ExecuteReaderAsync();
            return (await storedProcedure.GetDataSetAsync<Recipe>()).ToArray();
        }
        
        public async Task Update(Recipe recipe, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameUpdate, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", recipe.Id),
                    new StoredProcedureParameter("@name", recipe.Name),
                    new StoredProcedureParameter("@type", recipe.Type),
                    new StoredProcedureParameter("@text", recipe.Text),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }
    }
}