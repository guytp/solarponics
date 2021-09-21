using Refit;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.ApiClient
{
    public class SupplierApiClient : ISupplierApiClient
    {
        private readonly ISupplierApiClient refitClient;

        public SupplierApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<ISupplierApiClient>(settings.ApiBaseUrl, refitSettings);
        }
        
        public Task<Supplier> Add(Supplier supplier)
        {
            return this.refitClient.Add(supplier);
        }

        public Task<Supplier[]> Get()
        {
            return this.refitClient.Get();
        }
        
        public Task Update(int id, Supplier supplier)
        {
            return this.refitClient.Update(id, supplier);
        }
        
        public Task Delete(int id)
        {
            return this.refitClient.Delete(id);
        }
    }
}