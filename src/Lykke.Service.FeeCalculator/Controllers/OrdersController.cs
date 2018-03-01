using System.Net;
using System.Threading.Tasks;
using Lykke.Service.FeeCalculator.Core.Domain;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IFeeService _feeService;

        public OrdersController(
            IFeeService feeService
            )
        {
            _feeService = feeService;
        }

        /// <summary>
        /// Returns fee for the market order
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="assetPair"></param>
        /// <param name="assetId"></param>
        /// <param name="orderAction"></param>
        /// <returns></returns>
        [HttpGet("market")]
        [SwaggerOperation("GetMarketOrderFee")]
        [ProducesResponseType(typeof(MarketOrderFeeResponseModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMarketOrderFee([FromQuery] string clientId, [FromQuery] string assetPair,
            [FromQuery] string assetId,
            [FromQuery] OrderAction orderAction)
        {
            var fee = await _feeService.GetFeeAsync(clientId, assetPair, assetId);
            
            return Ok(new MarketOrderFeeResponseModel
            {
                DefaultFeeSize = fee.TakerFee
            });
        }

        /// <summary>
        /// Returns fee for the limit order
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="assetPair"></param>
        /// <param name="assetId"></param>
        /// <param name="orderAction"></param>
        /// <returns></returns>
        [HttpGet("limit")]
        [SwaggerOperation("GetLimitOrderFee")]
        [ProducesResponseType(typeof(LimitOrderFeeResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetLimitOrderFee([FromQuery] string clientId, [FromQuery] string assetPair,
            [FromQuery] string assetId,
            [FromQuery] OrderAction orderAction)
        {
            var fee = await _feeService.GetFeeAsync(clientId, assetPair, assetId);
            
            return Ok(new LimitOrderFeeResponseModel
            {
                TakerFeeSize = fee.TakerFee,
                MakerFeeSize = fee.MakerFee,
                TakerFeeType = fee.TakerFeeType,
                MakerFeeType = fee.MakerFeeType,
                MakerFeeModificator = fee.MakerFeeModificator
            });
        }
    }
}
