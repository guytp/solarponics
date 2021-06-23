using System.Threading.Tasks;
using Solarponics.Models;

namespace Solarponics.WebApi.Abstractions
{
    public interface IUserRepository
    {
        Task<User> Authenticate(short userId, short pin);
    }
}