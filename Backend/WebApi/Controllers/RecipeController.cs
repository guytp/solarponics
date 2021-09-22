using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

#pragma warning disable 1591

namespace Solarponics.WebApi.Controllers
{
    [Route("/recipes")]
    public class RecipeController : SolarponicsControllerBase
    {
        private readonly IRecipeRepository recipeRepository;

        public RecipeController(IRecipeRepository recipeRepository)
        {
            this.recipeRepository = recipeRepository;
        }

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(Recipe[]))]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await recipeRepository.Get());
        }

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(Recipe))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody]Recipe recipe)
        {
            recipe.Id = await recipeRepository.Add(recipe, this.UserId!.Value);

            return this.Ok(recipe);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody]Recipe recipe)
        {
            recipe.Id = id;
            await recipeRepository.Update(recipe, this.UserId!.Value);

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await recipeRepository.Delete(id, this.UserId!.Value);

            return this.NoContent();
        }
    }
}