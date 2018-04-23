using AutoMapper;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/[controller]")]
    public class WithdrawalFeesController : Controller
    {
        private readonly IWithdrawalFeesService _withdrawalFeesService;
//        private readonly List<WithdrawalFee> _settings;

        public WithdrawalFeesController(
            IWithdrawalFeesService withdrawalFeesService
            )
        {
            _withdrawalFeesService = withdrawalFeesService;
//            _settings = settings;
        }

        /// <summary>
        /// Returns fee for assetId and country
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        [HttpGet("{assetId}/{countryCode}")]
        [SwaggerOperation("GetWithdrawalFee")]
        [ProducesResponseType(typeof(WithdrawalFee), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetWithdrawalFee(string assetId, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(assetId))
            {
                return BadRequest("No asset defined");
            }
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return BadRequest("No country code defined");
            }

            var model = await _withdrawalFeesService.GetAsync(assetId);
            if (model == null)
            {
                return BadRequest($"No settings found for assetId: {assetId} and country: {countryCode}");
            }

            if (model.Countries != null && model.Countries.Contains(countryCode))
            {
                return Ok(new WithdrawalFee { AssetId = assetId, Size = model.SizeForSelectedCountries, PaymentSystem = model.PaymentSystemForSelectedCountries });
            }
            else
            {
                return Ok(new WithdrawalFee { AssetId = assetId, Size = model.SizeForOtherCountries, PaymentSystem = model.PaymentSystemForOtherCountries});
            }
        }

        [HttpGet("/WithdrawalFees")]
        [SwaggerOperation("GetWithdrawalFees")]
        [ProducesResponseType(typeof(List<WithdrawalFeeModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetWithdrawalFees()
        {
            var fees = await _withdrawalFeesService.GetAllAsync();
            return Ok(fees);
        }

        [HttpPost]
        [SwaggerOperation("SaveWithdrawalFee")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SaveWithdrawalFee([FromBody]WithdrawalFeeModel model)
        {
            await Task.FromResult(0);


            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            await _withdrawalFeesService.SaveAsync(model);

            return Ok();
        }


    }
}
