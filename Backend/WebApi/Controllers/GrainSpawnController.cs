using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.Models.WebApi;
using Solarponics.WebApi.Abstractions;

#pragma warning disable 1591

namespace Solarponics.WebApi.Controllers
{
    [Route("/grain-spawn")]
    public class GrainSpawnController : SolarponicsControllerBase
    {
        private readonly IGrainSpawnRepository repo;

        public GrainSpawnController(IGrainSpawnRepository recipeRepository)
        {
            this.repo = recipeRepository;
        }

        [HttpGet()]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GrainSpawn[]))]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            var returnValue = await repo.GetAll() ?? new GrainSpawn[0];
            return this.Ok(returnValue);
        }

        [HttpGet("by-id/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GrainSpawn))]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get(int id)
        {
            var returnValue = await repo.Get(id);
            if (returnValue == null)
            {
                return NotFound();
            }

            return this.Ok(returnValue);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GrainSpawn))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] GrainSpawnAddRequest request)
        {
            var id = await repo.Add(this.UserId!.Value, request.RecipeId, request.Weight, request.Notes);
            var grainSpawn = await repo.Get(id);
            return this.Ok(grainSpawn);
        }

        [HttpPost("by-id/{id}/innoculate")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GrainSpawn))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Innoculate(int id, [FromBody] GrainSpawnInnoculateRequest request)
        {
            await repo.Innoculate(id, request.CultureId, request.AdditionalNotes, this.UserId!.Value);
            var culture = await repo.Get(id);
            return this.Ok(culture);
        }

        [HttpPost("by-id/{id}/shelf")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GrainSpawn))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShelfPlace(int id, [FromBody] GrainSpawnShelfPlaceRequest request)
        {
            await repo.ShelfPlace(id, request.ShelfId, request.AdditionalNotes, this.UserId!.Value);
            var culture = await repo.Get(id);
            return this.Ok(culture);
        }
    }
}