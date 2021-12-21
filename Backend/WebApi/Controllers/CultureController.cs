using System;
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
    [Route("/cultures")]
    public class CultureController : SolarponicsControllerBase
    {
        private readonly ICultureRepository repo;

        public CultureController(ICultureRepository recipeRepository)
        {
            this.repo = recipeRepository;
        }

        [HttpGet()]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Culture[]))]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            var returnValue = await repo.GetAll() ?? new Culture[0];
            return this.Ok(returnValue);
        }

        [HttpGet("by-id/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Culture))]
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

        [HttpPut("book-in")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Culture))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BookIn([FromBody] CultureBookInRequest request)
        {
            var id = await repo.Add(request.SupplierId, null, this.UserId!.Value, null, request.MediumType, request.OrderDate, request.Strain, request.Notes, 1, DateTime.UtcNow);
            var culture = await repo.Get(id);
            return this.Ok(culture);
        }

        [HttpPut("from-recipe")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Culture))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FromRecipe([FromBody] CultureCreateFromReciptRequest request)
        {
            var id = await repo.Add(null, null, this.UserId!.Value, request.RecipeId, request.MediumType, null, null, request.Notes, null, request.Date);
            var culture = await repo.Get(id);
            return this.Ok(culture);
        }

        [HttpPost("innoculate")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Culture))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Innoculate([FromBody] CultureInnoculateRequest request)
        {
            await repo.Innoculate(request.Id, request.ParentCultureId, request.AdditionalNotes, this.UserId!.Value, request.Date);
            var culture = await repo.Get(request.Id);
            return this.Ok(culture);
        }
    }
}