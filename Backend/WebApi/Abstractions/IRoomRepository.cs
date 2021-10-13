using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Abstractions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IRoomRepository
    {
        Task<int> Add(int locationId, string name, int userId);

        Task<Room[]> Get();
    }
}