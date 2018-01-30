﻿using Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings;
using Lykke.Service.FeeCalculator.Models;
using Lykke.Service.FeeCalculator.Services.DummySettingsHolder;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("CashoutFees")]
    public class CashoutFeesController : Controller
    {
        private readonly IDummySettingsHolder _dummySettingsHolder;

        public CashoutFeesController(IDummySettingsHolder dummySettingsHolder)
        {
            _dummySettingsHolder = dummySettingsHolder;
        }

        [HttpGet]
        [SwaggerOperation("GetCashoutFees")]
        [ProducesResponseType(typeof(List<CashoutFee>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public IActionResult GetCashoutFees([FromQuery] string assetId = null)
        {
            if (string.IsNullOrWhiteSpace(assetId))
                return Ok(_dummySettingsHolder.GetCashoutFees());

            var assetIdUpper = assetId.ToUpper();
            var fees = _dummySettingsHolder.GetCashoutFees().Where(fee => fee.AssetId.ToUpper() == assetIdUpper)
                .ToList();

            if (fees.Count == 0)
                fees = new List<CashoutFee>
                {
                    new CashoutFee
                    {
                        AssetId = assetId,
                        Size = 0,
                        Type = FeeType.Absolute
                    }
                };

            return Ok(fees);
        }
    }
}
