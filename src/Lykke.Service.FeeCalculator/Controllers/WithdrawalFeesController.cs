using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using Lykke.Service.FeeCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Net;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/[controller]")]
    public class WithdrawalFeesController : Controller
    {
        private readonly FeeCalculatorSettings _settings;

        public WithdrawalFeesController(
            FeeCalculatorSettings settings
            )
        {
            _settings = settings;
        }

        /// <summary>
        /// Returns fee for assetId and country
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        [HttpGet("{assetId}/{countryCode}")]
        [SwaggerOperation("GetFee")]
        [ProducesResponseType(typeof(WithdrawalFeeModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetFee(string assetId, string countryCode)
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

            foreach (var withdrawalFeeSetting in _settings.WithdrawalFees)
            {
                if (withdrawalFeeSetting.AssetId == assetId)
                {
                    if (withdrawalFeeSetting.Countries != null)
                    {
                        if (withdrawalFeeSetting.Countries.Contains(countryCode))
                        {
                            return Ok(new WithdrawalFeeModel { AssetId = assetId, Size = withdrawalFeeSetting.Size, PaymentSystem = withdrawalFeeSetting.PaymentSystem });
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

            return Ok(new WithdrawalFeeModel { AssetId = assetId, Size = assetAllCountriesSetting.Size, PaymentSystem = assetAllCountriesSetting.PaymentSystem });
        }
    }
}
