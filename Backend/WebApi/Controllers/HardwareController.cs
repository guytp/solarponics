using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

#pragma warning disable 1591

namespace Solarponics.WebApi.Controllers
{
    [Route("/hardware")]
    public class HardwareController : SolarponicsControllerBase
    {
        private readonly IHardwareRepository hardwareRepository;

        public HardwareController(IHardwareRepository hardwareRepository)
        {
            this.hardwareRepository = hardwareRepository;
        }

        [HttpGet("by-machine-name/{machineName}")]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(HardwareSettings))]
        public async Task<IActionResult> Get(string machineName)
        {
            return this.Ok(await hardwareRepository.Get(machineName));
        }

        [HttpDelete("by-machine-name/{machineName}/barcode-scanner")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> RemoveBarcodeScanner(string machineName)
        {
            await hardwareRepository.RemoveBarcodeScanner(machineName, UserId!.Value);
            return this.NoContent();
        }

        [HttpPut("by-machine-name/{machineName}/barcode-scanner")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> SetBarcodeScanner(string machineName, [FromBody]BarcodeScannerSettings settings)
        {
            await hardwareRepository.SetBarcodeScanner(machineName, settings, UserId!.Value);
            return this.NoContent();
        }

        [HttpDelete("by-machine-name/{machineName}/label-printer/large")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> RemoveLabelPrinterLarge(string machineName)
        {
            await hardwareRepository.RemoveLabelPrinter(machineName, UserId!.Value, "Large");
            return this.NoContent();
        }

        [HttpDelete("by-machine-name/{machineName}/label-printer/small")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> RemoveLabelPrinterSmall(string machineName)
        {
            await hardwareRepository.RemoveLabelPrinter(machineName, UserId!.Value, "Small");
            return this.NoContent();
        }

        [HttpPut("by-machine-name/{machineName}/label-printer/small")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> SetLabelPrinterSmall(string machineName, [FromBody] LabelPrinterSettings settings)
        {
            await hardwareRepository.SetLabelPrinter(machineName, settings, UserId!.Value, "Small");
            return this.NoContent();
        }

        [HttpPut("by-machine-name/{machineName}/label-printer/large")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> SetLabelPrinterLarge(string machineName, [FromBody] LabelPrinterSettings settings)
        {
            await hardwareRepository.SetLabelPrinter(machineName, settings, UserId!.Value, "Large");
            return this.NoContent();
        }

        [HttpDelete("by-machine-name/{machineName}/scale")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> RemoveScale(string machineName)
        {
            await hardwareRepository.RemoveScale(machineName, UserId!.Value);
            return this.NoContent();
        }

        [HttpPut("by-machine-name/{machineName}/scale")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> SetScale(string machineName, [FromBody]ScaleSettings settings)
        {
            await hardwareRepository.SetScale(machineName, settings, UserId!.Value);
            return this.NoContent();
        }
    }
}