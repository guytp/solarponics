using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class SupplierRepository : ISupplierRepository
    {
        private const string ProcedureNameAdd = "[dbo].[SupplierAdd]";
        private const string ProcedureNameDelete = "[dbo].[SupplierDelete]";
        private const string ProcedureNameGet = "[dbo].[SupplierGet]";
        private const string ProcedureNameUpdate = "[dbo].[SupplierUpdate]";

        public SupplierRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }
        
        public async Task<int> Add(Supplier supplier, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAdd, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@name", supplier.Name),
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

        public async Task<Supplier[]> Get()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, null);
            await storedProcedure.ExecuteReaderAsync();
            return (await storedProcedure.GetDataSetAsync<Supplier>()).ToArray();
        }
        
        public async Task Update(Supplier supplier, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameUpdate, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@id", supplier.Id),
                    new StoredProcedureParameter("@name", supplier.Name),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }
    }
}