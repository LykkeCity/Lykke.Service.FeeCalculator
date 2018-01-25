using System.Net;
using Lykke.Service.FeeCalculator.Core.Domain;
using Lykke.Service.FeeCalculator.Models;
using Lykke.Service.FeeCalculator.Services.DummySettingsHolder;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IDummySettingsHolder _dummySettingsHolder;

        public OrdersController(IDummySettingsHolder dummySettingsHolder)
        {
            _dummySettingsHolder = dummySettingsHolder;
        }

        [HttpGet("market")]
        [SwaggerOperation("GetMarketOrderFee")]
        [ProducesResponseType(typeof(MarketOrderFeeResponseModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public IActionResult GetMarketOrderFee([FromQuery] string clientId, [FromQuery] string assetPair,
            [FromQuery] string assetId,
            [FromQuery] OrderAction orderAction)
        {
            return Ok(new MarketOrderFeeResponseModel
            {
                DefaultFeeSize = _dummySettingsHolder.GetMarkerOrderSettings().DefaultFeeSize
            });
        }

        [HttpGet("limit")]
        [SwaggerOperation("GetLimitOrderFee")]
        [ProducesResponseType(typeof(LimitOrderFeeResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public IActionResult GetLimitOrderFee([FromQuery] string clientId, [FromQuery] string assetPair,
            [FromQuery] string assetId,
            [FromQuery] OrderAction orderAction)
        {
            return Ok(new LimitOrderFeeResponseModel
            {
                TakerFeeSize = _dummySettingsHolder.GetLimitOrderSettings().TakerFeeSize,
                MakerFeeSize = _dummySettingsHolder.GetLimitOrderSettings().MakerFeeSize
            });
        }
    }
}
