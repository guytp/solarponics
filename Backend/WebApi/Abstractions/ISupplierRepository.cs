using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Abstractions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ISupplierRepository
    {
        Task<int> Add(Supplier supplier, int userId);

        Task Delete(int id, int userId);

        Task<Supplier[]> Get();

        Task Update(Supplier supplier, int userId);
    }
}