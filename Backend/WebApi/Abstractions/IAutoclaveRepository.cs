using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Abstractions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IAutoclaveRepository
    {
        Task<int> Add(Autoclave autoclave, int userId);

        Task Delete(int id, int userId);

        Task<Autoclave[]> Get();
    }
}