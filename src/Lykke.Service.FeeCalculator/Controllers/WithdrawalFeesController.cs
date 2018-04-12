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
        private readonly List<WithdrawalFee> _settings;

        public WithdrawalFeesController(
            IWithdrawalFeesService withdrawalFeesService,
            List<WithdrawalFee> settings
            )
        {
            _withdrawalFeesService = withdrawalFeesService;
            _settings = settings;
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
        public IActionResult GetWithdrawalFee(string assetId, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(assetId))
            {
                return BadRequest("No asset defined");
            }
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return BadRequest("No country code defined");
            }

            WithdrawalFee assetAllCountriesSetting = null;

            foreach (var withdrawalFeeSetting in _settings)
            {
                if (withdrawalFeeSetting.AssetId == assetId)
                {
                    if (withdrawalFeeSetting.Countries != null)
                    {
                        if (withdrawalFeeSetting.Countries.Contains(countryCode))
                        {
                            return Ok(new WithdrawalFee { AssetId = assetId, Size = withdrawalFeeSetting.Size, PaymentSystem = withdrawalFeeSetting.PaymentSystem });
                        }
                    }
                    else
                    {
                        assetAllCountriesSetting = withdrawalFeeSetting;
                    }
                }
            }

            if (assetAllCountriesSetting == null)
            {
                return BadRequest($"No settings found for assetId: {assetId} and country: {countryCode}");
            }

            return Ok(new WithdrawalFee { AssetId = assetId, Size = assetAllCountriesSetting.Size, PaymentSystem = assetAllCountriesSetting.PaymentSystem });
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
        public async Task<IActionResult> SaveWithdrawalFee([FromBody]List<WithdrawalFeeModel> model)
        {
            await Task.FromResult(0);


            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            await _withdrawalFeesService.SaveAsync(model);

            return Ok();
        }


    }
}
