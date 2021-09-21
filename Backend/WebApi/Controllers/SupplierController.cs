using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

#pragma warning disable 1591

namespace Solarponics.WebApi.Controllers
{
    [Route("/suppliers")]
    public class SupplierController : SolarponicsControllerBase
    {
        private readonly ISupplierRepository supplierRepository;

        public SupplierController(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(Supplier[]))]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await supplierRepository.Get());
        }

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(Supplier))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody]Supplier supplier)
        {
            supplier.Id = await supplierRepository.Add(supplier, this.UserId!.Value);

            return this.Ok(supplier);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody]Supplier supplier)
        {
            supplier.Id = id;
            await supplierRepository.Update(supplier, this.UserId!.Value);

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await supplierRepository.Delete(id, this.UserId!.Value);

            return this.NoContent();
        }
    }
}