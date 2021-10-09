using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Controllers
{
    /// <summary>
    ///     Provides access to information about sensor modules.
    /// </summary>
    [Route("sensor-modules")]
    public class SensorModuleController : ControllerBase
    {
        private readonly ISensorModuleRepository _sensorModuleRepository;

#pragma warning disable 1591
        public SensorModuleController(ISensorModuleRepository sensorModuleRepository)
#pragma warning restore 1591
        {
            _sensorModuleRepository = sensorModuleRepository;
        }

        /// <summary>
        ///     Gets a list of all sensor modules.
        /// </summary>
        /// <returns>
        ///     An OK result containing all sensor modules.
        /// </returns>
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(SensorModule[]))]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            var results = await _sensorModuleRepository.GetAll();
            return results == null ? NotFound() : (IActionResult) Ok(results);
        }
    }
}