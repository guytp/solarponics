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
    [Route("/fruiting-blocks")]
    public class FruitingBlockController : SolarponicsControllerBase
    {
        private readonly IFruitingBlockRepository repo;

        public FruitingBlockController(IFruitingBlockRepository recipeRepository)
        {
            this.repo = recipeRepository;
        }

        [HttpGet()]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FruitingBlock[]))]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            var returnValue = await repo.GetAll() ?? new FruitingBlock[0];
            return this.Ok(returnValue);
        }

        [HttpGet("by-id/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FruitingBlock))]
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FruitingBlock))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] FruitingBlockAddRequest request)
        {
            var id = await repo.Add(this.UserId!.Value, request.RecipeId, request.Weight, request.Date, request.Notes);
            var fruitingBlock = await repo.Get(id);
            return this.Ok(fruitingBlock);
        }

        [HttpPost("by-id/{id}/innoculate")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FruitingBlock))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Innoculate(int id, [FromBody] FruitingBlockInnoculateRequest request)
        {
            await repo.Innoculate(id, request.GrainSpawnId, request.AdditionalNotes, request.Date, this.UserId!.Value);
            var culture = await repo.Get(id);
            return this.Ok(culture);
        }

        [HttpPost("by-id/{id}/shelf/incubate")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FruitingBlock))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShelfPlaceIncubate(int id, [FromBody] FruitingBlockShelfPlaceRequest request)
        {
            await repo.ShelfPlaceIncubate(id, request.ShelfId, request.AdditionalNotes, request.Date, this.UserId!.Value);
            var culture = await repo.Get(id);
            return this.Ok(culture);
        }

        [HttpPost("by-id/{id}/shelf/fruiting")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FruitingBlock))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShelfPlaceFruiting(int id, [FromBody] FruitingBlockShelfPlaceRequest request)
        {
            await repo.ShelfPlaceFruiting(id, request.ShelfId, request.AdditionalNotes, request.Date, this.UserId!.Value);
            var culture = await repo.Get(id);
            return this.Ok(culture);
        }
    }
}