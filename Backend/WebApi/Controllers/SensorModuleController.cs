using System.Net;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Get()
        {
            var results = await _sensorModuleRepository.GetAll();
            return results == null ? NotFound() : (IActionResult) Ok(results);
        }

        /// <summary>
        ///     Gets a list of all sensor modules that are approved for provisioning but not yet provisioned.
        /// </summary>
        /// <returns>
        ///     An OK result containing all configs of pending modules.
        /// </returns>
        [HttpGet("provisioning-queue")]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(SensorModuleConfig[]))]
        public async Task<IActionResult> ProvisioningQueueGet()
        {
            var results = await _sensorModuleRepository.ProvisionQueueGetAll();
            return Ok(results ?? new SensorModuleConfig[0]);
        }

        /// <summary>
        ///     Adds the details of a sensor that can be provisioned.
        /// </summary>
        /// <param name="config">
        ///     The configuration for the new sensor.
        /// </param>
        /// <returns>
        ///     A NoContent response.
        /// </returns>
        [HttpPost("provisioning-queue")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> ProvisioningQueueAdd([FromBody] SensorModuleConfig config)
        {
            await _sensorModuleRepository.ProvisioningQueueAdd(config);
            return NoContent();
        }

        /// <summary>
        ///     Removes a sensor module that is pending provisioning.
        /// </summary>
        /// <param name="serialNumber">
        ///     The serial number of the sensor module.
        /// </param>
        /// <returns>
        ///     A NoContent response if successful or NotFound if the serial number was not found.
        /// </returns>
        [HttpDelete("provisioning-queue/{serialNumber}")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> ProvisioningQueueDelete([FromRoute] string serialNumber)
        {
            var result = await _sensorModuleRepository.ProvisionQueueGetBySerialNumber(serialNumber);

            if (result == null) return NotFound();

            await _sensorModuleRepository.ProvisioningQueueDelete(serialNumber);

            return NoContent();
        }
    }
}