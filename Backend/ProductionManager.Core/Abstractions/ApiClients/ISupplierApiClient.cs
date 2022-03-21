using Refit;
using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.Abstractions.ApiClients
{
    public interface ISupplierApiClient
    {
        [Headers("Authorization: Bearer")]
        [Post("/suppliers")]
        Task<Supplier> Add([Body]Supplier supplier);

        [Headers("Authorization: Bearer")]
        [Get("/suppliers")]
        Task<Supplier[]> Get();
        
        [Headers("Authorization: Bearer")]
        [Put("/suppliers/{id}")]
        Task Update(int id, [Body]Supplier supplier);
        
        [Headers("Authorization: Bearer")]
        [Delete("/suppliers/{id}")]
        Task Delete(int id);
    }
}