using Lykke.Service.FeeCalculator.Models;
using Microsoft.AspNetCore.Mvc;    
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.Assets.Client;
using Lykke.Service.FeeCalculator.Core.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using CashoutFee = Lykke.Service.FeeCalculator.Models.CashoutFee;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/cashoutfees")]
    public class CashoutFeesController : Controller
    {
        private readonly ICashoutFeesService _cashoutFeesService;
        private readonly IAssetsServiceWithCache _assetsServiceWithCache;

        public CashoutFeesController(
            ICashoutFeesService cashoutFeesService,
            IAssetsServiceWithCache assetsServiceWithCache
            )
        {
            _cashoutFeesService = cashoutFeesService;
            _assetsServiceWithCache = assetsServiceWithCache;
        }

        [HttpGet("/CashoutFees")]
        [SwaggerOperation("GetCashoutFees")]
        [ProducesResponseType(typeof(List<CashoutFee>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCashoutFees(string assetId = null)
        {
            var fees = await _cashoutFeesService.GetAllAsync();
            
            if (!string.IsNullOrEmpty(assetId))
                fees = fees.Where(item => item.AssetId == assetId).ToArray();
            
            var result = Mapper.Map<List<CashoutFee>>(fees);
            return Ok(result);
        }
        
        [HttpPost]
        [SwaggerOperation("AddCashoutFee")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddCashoutFees([FromBody]CashoutFeeModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));
            
            if (!string.IsNullOrEmpty(model.Id) && !model.Id.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Id)} value"));
            
            var asset = await _assetsServiceWithCache.TryGetAssetAsync(model.AssetId);

            if (asset == null)
                return NotFound(ErrorResponse.Create($"asset '{model.AssetId}' not found"));

            var fees = await _cashoutFeesService.GetAllAsync();

            if (fees.Any(item => item.AssetId == model.AssetId))
                return BadRequest($"fee for asset '{model.AssetId}' is already added");

            await _cashoutFeesService.AddAsync(new Core.Domain.CashoutFee.CashoutFee
            {
                Id = model.Id,
                AssetId = model.AssetId, 
                Size = model.Size, 
                Type = model.Type
            });

            return Ok();
        }
        
        [HttpDelete("{id}")]
        [SwaggerOperation("DeleteCashoutFee")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteCashoutFees(string id)
        {
            if (!id.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(id)} value"));
            
            await _cashoutFeesService.DeleteAsync(id);

            return Ok();
        }
    }
}
