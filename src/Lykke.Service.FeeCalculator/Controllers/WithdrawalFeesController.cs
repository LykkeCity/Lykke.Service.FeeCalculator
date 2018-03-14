using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using Lykke.Service.FeeCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Net;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/[controller]")]
    public class WithdrawalFeesController : Controller
    {
        private readonly List<WithdrawalFee> _settings;

        public WithdrawalFeesController(
            List<WithdrawalFee> settings
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
        [SwaggerOperation("GetWithdrawalFee")]
        [ProducesResponseType(typeof(WithdrawalFeeModel), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
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
