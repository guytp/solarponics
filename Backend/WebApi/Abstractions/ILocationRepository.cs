using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Abstractions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ILocationRepository
    {
        Task<int> Add(string name, int userId);

        Task<Location[]> Get();
    }
}