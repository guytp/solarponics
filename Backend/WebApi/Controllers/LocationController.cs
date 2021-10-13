using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

#pragma warning disable 1591

namespace Solarponics.WebApi.Controllers
{
    [Route("/locations")]
    public class LocationController : SolarponicsControllerBase
    {
        private readonly ILocationRepository locationRepository;
        private readonly IRoomRepository roomRepository;

        public LocationController(ILocationRepository locationRepository, IRoomRepository roomRepository)
        {
            this.locationRepository = locationRepository;
            this.roomRepository = roomRepository;
        }

        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(Location[]))]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get()
        {
            var locations = (await locationRepository.Get()) ?? new Location[0];
            var allRooms = (await this.roomRepository.Get()) ?? new Room[0];
            foreach (var location in locations)
            {
                location.Rooms = allRooms.Where(r => r.LocationId == location.Id).ToArray();
            }

            return this.Ok(locations);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(int))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] Location location)
        {
            if (location == null)
            {
                return this.ValidationFailure("Location is required", nameof(location));
            }

            if (location.Id > 0)
            {
                return this.ValidationFailure("Id cannot be supplied in body", nameof(location.Id));
            }

            if (location.Rooms != null && location.Rooms.Length > 0)
            {
                return this.ValidationFailure("Rooms cannot be supplied in body", nameof(location.Rooms));
            }

            var id = await locationRepository.Add(location.Name, UserId.Value);
            return this.Ok(id);
        }

        [HttpPut("by-id/{locationId}/rooms")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(int))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoom(int locationId, [FromBody] Room room)
        {

            if (room == null)
            {
                return this.ValidationFailure("Room is required", nameof(room));
            }
            if (room.Id > 0)
            {
                return this.ValidationFailure("Id cannot be supplied in body", nameof(room.Id));
            }

            if (room.LocationId != 0 && room.LocationId != locationId)
            {
                return this.ValidationFailure("LocationId on body must be 0 or equal to query string", nameof(room.LocationId));
            }

            var id = await roomRepository.Add(locationId, room.Name, UserId.Value);
            return this.Ok(id);
        }
    }
}