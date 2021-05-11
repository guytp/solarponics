using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Controllers
{
    /// <summary>
    ///     Get data to display on charts and reports.
    /// </summary>
    [Route("readings")]
    public class SensorReadingController : ControllerBase
    {
        private readonly ISensorReadingRepository _sensorReadingRepository;

#pragma warning disable 1591
        public SensorReadingController(ISensorReadingRepository sensorReadingRepository)
#pragma warning restore 1591
        {
            _sensorReadingRepository = sensorReadingRepository;
        }

        /// <summary>
        ///     Gets aggregate sensor data to use for charting.
        /// </summary>
        /// <param name="id">The sensor to get the data for.</param>
        /// <param name="timeframe">The timeframe to aggregate data by.</param>
        /// <returns>An array of SensorReading objects to use for charting.</returns>
        [HttpGet("by-sensor-id/{id}/aggregate-by/{timeframe}")]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(SensorReading[]))]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAggregate(int id, AggregateTimeframe timeframe)
        {
            var results = await _sensorReadingRepository.GetReadings(id, timeframe);
            return results == null ? (IActionResult) NotFound() : Ok(results);
        }
    }
}