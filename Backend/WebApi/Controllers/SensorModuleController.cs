using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Controllers
{
#pragma warning disable 1591
    [Route("sensor-modules")]
    public class SensorModuleController : SolarponicsControllerBase
    {
        private readonly ISensorModuleRepository _sensorModuleRepository;
        private readonly ILocationRepository locationRepository;
        private readonly IRoomRepository roomRepository;

        public SensorModuleController(ISensorModuleRepository sensorModuleRepository, ILocationRepository locationRepository, IRoomRepository roomRepository)
        {
            _sensorModuleRepository = sensorModuleRepository;
            this.locationRepository = locationRepository;
            this.roomRepository = roomRepository;
        }

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(SensorModule[]))]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            var results = await _sensorModuleRepository.GetAll();
            return Ok(results ?? new SensorModule[0]);
        }

        [HttpGet("by-type/modbus-tcp")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SensorModuleModbusTcp[]))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetModbusTcp()
        {
            var results = await _sensorModuleRepository.GetModbusTcp();
            return Ok(results ?? new SensorModuleModbusTcp[0]);
        }

        [HttpDelete("by-id/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _sensorModuleRepository.Delete(id);
            return NoContent();
        }

        [HttpPut("by-type/modbus-tcp")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(int))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody]SensorModuleModbusTcp sensorModule)
        {
            if (sensorModule == null)
            {
                return this.ValidationFailure("SensorModule must be supplied", nameof(sensorModule));
            }

            var locations = await this.locationRepository.Get();
            var location = locations.FirstOrDefault(l => l.Name == sensorModule.Location);
            if (location == null)
            {
                return this.ValidationFailure("Location not found", nameof(sensorModule.Location));
            }

            if (locations.Count(l => l.Name == sensorModule.Location) > 1)
            {
                return this.ValidationFailure("Multiple locations with same name found", nameof(sensorModule.Location));
            }

            var rooms = (await this.roomRepository.Get())?.Where(r => r.LocationId == location.Id)?.ToArray();
            var room = rooms?.FirstOrDefault(r => r.Name == sensorModule.Room);
            if (room == null)
            {
                return this.ValidationFailure("Room not found", nameof(sensorModule.Room));
            }

            if (rooms.Count(l => l.Name == sensorModule.Room) > 1)
            {
                return this.ValidationFailure("Multiple rooms with same name found", nameof(sensorModule.Room));
            }

            var allExisting = await this._sensorModuleRepository.GetAll();
            if (allExisting.Any(sm => sm.Name == sensorModule.Name))
            {
                return this.ValidationFailure("Name already in use", nameof(sensorModule.Name));
            }

            if (allExisting.Any(sm => sm.SerialNumber == sensorModule.SerialNumber))
            {
                return this.ValidationFailure("SerialNumber already in use", nameof(sensorModule.SerialNumber));
            }

            var existingModbusTcp = (await this._sensorModuleRepository.GetModbusTcp()).Any(sm => sm.IpAddress == sensorModule.IpAddress && sm.Port == sensorModule.Port);
            if (existingModbusTcp)
            {
                return this.ValidationFailure("IpAddress/Port already in use", nameof(sensorModule.IpAddress));
            }

            var temperatureSensor = sensorModule.Sensors.FirstOrDefault(s => s.Type == SensorType.Temperature);
            var carbonDioxideSensor = sensorModule.Sensors.FirstOrDefault(s => s.Type == SensorType.CarbonDioxide);
            var humiditySensor = sensorModule.Sensors.FirstOrDefault(s => s.Type == SensorType.Humidity);

            if (temperatureSensor == null && carbonDioxideSensor == null && humiditySensor == null)
            {
                return this.ValidationFailure("At least one supported sensor must be supplied", nameof(sensorModule.Sensors));
            }

            var id = await _sensorModuleRepository.AddModbusTcp(
                room.Id,
                sensorModule.SerialNumber,
                sensorModule.Name,
                sensorModule.IpAddress,
                sensorModule.Port,
                UserId!.Value,
                temperatureSensor == null ? (int?)null : temperatureSensor.Number,
                temperatureSensor == null ? (decimal?)null : temperatureSensor.WarningLowBelow,
                temperatureSensor == null ? (decimal?)null : temperatureSensor.CriticalLowBelow,
                temperatureSensor == null ? (decimal?)null : temperatureSensor.WarningHighAbove,
                temperatureSensor == null ? (decimal?)null : temperatureSensor.CriticalHighAbove,
                humiditySensor == null ? (int?)null : humiditySensor.Number,
                humiditySensor == null ? (decimal?)null : humiditySensor.WarningLowBelow,
                humiditySensor == null ? (decimal?)null : humiditySensor.CriticalLowBelow,
                humiditySensor == null ? (decimal?)null : humiditySensor.WarningHighAbove,
                humiditySensor == null ? (decimal?)null : humiditySensor.CriticalHighAbove,
                carbonDioxideSensor == null ? (int?)null : carbonDioxideSensor.Number,
                carbonDioxideSensor == null ? (decimal?)null : carbonDioxideSensor.WarningLowBelow,
                carbonDioxideSensor == null ? (decimal?)null : carbonDioxideSensor.CriticalLowBelow,
                carbonDioxideSensor == null ? (decimal?)null : carbonDioxideSensor.WarningHighAbove,
                carbonDioxideSensor == null ? (decimal?)null : carbonDioxideSensor.CriticalHighAbove
                );
            return Ok(id);
        }
    }
}