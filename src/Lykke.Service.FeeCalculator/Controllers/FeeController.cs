using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/[controller]")]
    public class FeeController : Controller
    {
        private readonly IFeeRepository _feeRepository;
        private readonly IStaticFeeRepository _staticFeeRepository;
        private readonly CachedDataDictionary<decimal, IFee> _feesCache;
        private readonly CachedDataDictionary<string, IStaticFee> _feesStaticCache;

        public FeeController(
            IFeeRepository feeRepository, 
            IStaticFeeRepository staticFeeRepository,
            CachedDataDictionary<decimal, IFee> feesCache,
            CachedDataDictionary<string, IStaticFee> feesStaticCache
            )
        {
            _feeRepository = feeRepository;
            _staticFeeRepository = staticFeeRepository;
            _feesCache = feesCache;
            _feesStaticCache = feesStaticCache;
        }

        [HttpPost("add")]
        [SwaggerOperation("AddFee")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddFee([FromBody]FeeModel model)
        {
            await _feeRepository.AddFeeAsync(new Fee
            {
                Volume = model.Volume,
                MakerFee = model.MakerFee,
                TakerFee = model.TakerFee
            });
            
            _feesCache.Invalidate();

            return Ok(true);
        }
        
        [HttpGet("get")]
        [SwaggerOperation("GetFees")]
        [ProducesResponseType(typeof(List<Fee>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetFees()
        {
            var fees = await _feeRepository.GetFeesAsync();
            return Ok(fees);
        }
        
        [HttpPost("delete/{volume}")]
        [SwaggerOperation("DeleteFee")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteFee([FromRoute]decimal volume)
        {
            await _feeRepository.DeleteFeeAsync(volume);
            _feesCache.Invalidate();
            return Ok(true);
        }
        
        [HttpPost("staticfee/add")]
        [SwaggerOperation("AddStaticFee")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddStaticFee([FromBody]StaticFeeModel model)
        {
            await _staticFeeRepository.AddFeeAsync(new StaticFee
            {
                AssetPair = model.AssetPair,
                MakerFee = model.MakerFee,
                TakerFee = model.TakerFee
            });
            
            _feesStaticCache.Invalidate();

            return Ok(true);
        }
        
        [HttpGet("staticfee/get")]
        [SwaggerOperation("GetStaticFees")]
        [ProducesResponseType(typeof(List<StaticFee>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetStaticFees()
        {
            var fees = await _staticFeeRepository.GetFeesAsync();
            return Ok(fees);
        }
        
        [HttpPost("staticfee/delete/{assetPair}")]
        [SwaggerOperation("DeleteStaticFee")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteStaticFee(string assetPair)
        {
            await _staticFeeRepository.DeleteFeeAsync(assetPair);
            _feesStaticCache.Invalidate();
            return Ok(true);
        }
    }
}
