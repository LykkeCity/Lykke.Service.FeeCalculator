using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.Assets.Client;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/marketorderassetfees")]
    public class MarketOrderAssetFeeController : Controller
    {
        private readonly IMarketOrderAssetFeeService _marketOrderAssetFeeService;
        private readonly IAssetsServiceWithCache _assetsServiceWithCache;

        public MarketOrderAssetFeeController(
            IMarketOrderAssetFeeService marketOrderAssetFeeService,
            IAssetsServiceWithCache assetsServiceWithCache
            )
        {
            _marketOrderAssetFeeService = marketOrderAssetFeeService;
            _assetsServiceWithCache = assetsServiceWithCache;
        }
        
        /// <summary>
        /// Adds a market order asset fee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation("AddMarketOrderAssetFee")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddMarketOrderAssetFee([FromBody]MoAssetFeeModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!string.IsNullOrEmpty(model.Id) && !model.Id.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Id)} value"));
            
            var asset = await _assetsServiceWithCache.TryGetAssetAsync(model.AssetId);

            if (asset == null)
                return NotFound(ErrorResponse.Create($"asset '{model.AssetId}' not found"));
            
            var fees = await _marketOrderAssetFeeService.GetAllAsync();

            if (fees.Any(item => item.AssetId == model.AssetId))
                return BadRequest($"fee for asset '{model.AssetId}' is already added");
            
            await _marketOrderAssetFeeService.AddAsync(new Core.Domain.MarketOrderAssetFee.MarketOrderAssetFee
            {
                Id = model.Id,
                Amount = model.Amount,
                AssetId = model.AssetId,
                Type = model.Type,
                TargetAssetId = model.TargetAssetId,
                TargetWalletId = model.TargetWalletId
            });
            
            return Ok();
        }
        
        /// <summary>
        /// Gets all the market order asset fees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("GetMarketOrderAssetFees")]
        [ProducesResponseType(typeof(List<MoAssetFee>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMarketOrderAssetFees()
        {
            var fees = await _marketOrderAssetFeeService.GetAllAsync();
            var result = Mapper.Map<List<MoAssetFee>>(fees);
            return Ok(result);
        }
        
        /// <summary>
        /// Deletes the market order asset fee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerOperation("DeleteMarketOrderAssetFee")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteMarketOrderAssetFee(string id)
        {
            if (!id.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(id)} value"));
            
            await _marketOrderAssetFeeService.DeleteAsync(id);
            return Ok();
        }
    }
}
