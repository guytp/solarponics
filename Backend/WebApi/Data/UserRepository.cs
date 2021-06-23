using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class UserRepository : IUserRepository
    {
        private const string ProcedureNameAuthenticate = "[dbo].[UserAuthenticate]";

        public UserRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<User> Authenticate(short userId, short pin)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAuthenticate, new[]
            {
                new StoredProcedureParameter("userId", userId),
                new StoredProcedureParameter("pin", pin)
            });

            await storedProcedure.ExecuteReaderAsync();
            return await storedProcedure.GetFirstOrDefaultRowAsync<User>();
        }
    }
}