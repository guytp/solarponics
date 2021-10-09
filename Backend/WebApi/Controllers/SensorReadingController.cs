using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Controllers
{
    [Route("readings")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class SensorReadingController : ControllerBase
    {
        private readonly ISensorReadingRepository _sensorReadingRepository;

        public SensorReadingController(ISensorReadingRepository sensorReadingRepository)
        {
            _sensorReadingRepository = sensorReadingRepository;
        }

        [HttpGet("by-sensor-id/{id}/aggregate-by/{timeframe}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AggregateSensorReading[]))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAggregate(int id, AggregateTimeframe timeframe)
        {
            var results = await _sensorReadingRepository.GetReadings(id, timeframe);
            return results == null ? (IActionResult)NotFound() : Ok(results);
        }

        [HttpGet("by-sensor-id/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SensorReading))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetCurrent(int id)
        {
            var results = await _sensorReadingRepository.GetCurrentReading(id);
            var response = results == null ? (IActionResult)NotFound() : Ok(results);
            return response;
        }
    }
}