using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

#pragma warning disable 1591

namespace Solarponics.WebApi.Controllers
{
    [Route("/shelves")]
    public class ShelfController : SolarponicsControllerBase
    {
        private readonly IShelfRepository repo;

        public ShelfController(IShelfRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(Shelf[]))]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await repo.Get());
        }

        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(int))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody]Shelf shelf)
        {
            var id = await repo.Add(shelf, this.UserId!.Value);

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