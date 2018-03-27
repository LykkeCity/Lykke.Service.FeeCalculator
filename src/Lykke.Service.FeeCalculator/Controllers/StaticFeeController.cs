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
using StaticFee = Lykke.Service.FeeCalculator.Models.StaticFee;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/staticfees")]
    public class StaticFeeController : Controller
    {
        private readonly IStaticFeeService _staticFeeService;
        private readonly IAssetsServiceWithCache _assetsServiceWithCache;

        public StaticFeeController(
            IStaticFeeService staticFeeService,
            IAssetsServiceWithCache assetsServiceWithCache
            )
        {
            _staticFeeService = staticFeeService;
            _assetsServiceWithCache = assetsServiceWithCache;
        }
        
        /// <summary>
        /// Adds a static fee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation("AddStaticFee")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddStaticFee([FromBody]StaticFeeModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));
            
            if (!string.IsNullOrEmpty(model.Id) && !model.Id.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Id)} value"));
            
            var assetpair = await _assetsServiceWithCache.TryGetAssetPairAsync(model.AssetPair);

            if (assetpair == null)
                return NotFound(ErrorResponse.Create($"asset pair '{model.AssetPair}' not found"));
            
            var fees = await _staticFeeService.GetAllAsync();

            if (fees.Any(item => item.AssetPair == model.AssetPair))
                return BadRequest($"fee for asset pair '{model.AssetPair}' is already added");
            
            await _staticFeeService.AddAsync(new Core.Domain.Fees.StaticFee
            {
                Id = model.Id,
                AssetPair = model.AssetPair,
                MakerFee = model.MakerFee,
                TakerFee = model.TakerFee,
                MakerFeeType = model.MakerFeeType,
                TakerFeeType = model.TakerFeeType,
                MakerFeeModificator = model.MakerFeeModificator
            });
            
            return Ok();
        }
        
        /// <summary>
        /// Gets all the static fees
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("GetStaticFees")]
        [ProducesResponseType(typeof(List<StaticFee>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetStaticFees()
        {
            var fees = await _staticFeeService.GetAllAsync();
            var result = Mapper.Map<List<StaticFee>>(fees);
            return Ok(result);
        }
        
        /// <summary>
        /// Deletes the static fee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerOperation("DeleteStaticFee")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteStaticFee(string id)
        {
            if (!id.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(id)} value"));
            
            await _staticFeeService.DeleteAsync(id);
            return Ok();
        }
    }
}
