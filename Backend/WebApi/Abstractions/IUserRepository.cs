using System.Threading.Tasks;
using Solarponics.Models;

namespace Solarponics.WebApi.Abstractions
{
    #pragma warning disable CS1591
    public interface IUserRepository
    {
        Task<User> Authenticate(short userId, short pin);
    }
}