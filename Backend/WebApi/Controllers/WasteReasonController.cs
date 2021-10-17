using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

#pragma warning disable 1591

namespace Solarponics.WebApi.Controllers
{
    [Route("/waste-reasons")]
    public class WasteReasonController : SolarponicsControllerBase
    {
        private readonly IWasteReasonRepository repo;

        public WasteReasonController(IWasteReasonRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(WasteReason[]))]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await repo.Get());
        }

        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(int))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody]WasteReason wasteReason)
        {
            var id = await repo.Add(wasteReason, this.UserId!.Value);

            return this.Ok(id);
        }

        [HttpDelete("by-id/{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await repo.Delete(id, this.UserId!.Value);

            return this.NoContent();
        }
    }
}