using System.Net;
using Lykke.Service.FeeCalculator.Core.Domain;
using Lykke.Service.FeeCalculator.Models;
using Lykke.Service.FeeCalculator.Services.DummySettingsHolder;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/[controller]")]
    public class TradeController : Controller
    {
        private readonly IDummySettingsHolder _dummySettingsHolder;

        public TradeController(IDummySettingsHolder dummySettingsHolder)
        {
            _dummySettingsHolder = dummySettingsHolder;
        }

        [HttpGet]
        [SwaggerOperation("GetTradeFee")]
        [ProducesResponseType(typeof(FeeResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public IActionResult Get([FromQuery] string clientId, [FromQuery] string assetPair, [FromQuery] string assetId,
            [FromQuery] OrderAction orderAction)
        {
            return Ok(new FeeResponse
            {
                Value = _dummySettingsHolder.GetTradeSettings().DefaultFee
            });
        }
    }
}
