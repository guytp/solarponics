using System;
using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class LocationRepository : ILocationRepository
    {
        private const string ProcedureNameGet = "[dbo].[LocationGet]";
        private const string ProcedureNameAdd = "[dbo].[LocationAdd]";

        public LocationRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<Location[]> Get()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, null);
            await storedProcedure.ExecuteReaderAsync();
            return (await storedProcedure.GetDataSetAsync<Location>()).ToArray();
        }

        public async Task<int> Add(string name, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAdd, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@name", name),
                    new StoredProcedureParameter("@userId", userId)
                });
            return await storedProcedure.ExecuteScalarAsync<int>();
        }
    }
}