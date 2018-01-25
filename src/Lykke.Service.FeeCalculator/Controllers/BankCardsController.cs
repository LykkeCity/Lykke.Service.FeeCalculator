using System;
using System.Net;
using Lykke.Service.FeeCalculator.Models;
using Lykke.Service.FeeCalculator.Services.DummySettingsHolder;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/[controller]")]
    public class BankCardsController : Controller
    {
        private readonly IDummySettingsHolder _dummySettingsHolder;

        public BankCardsController(
            IDummySettingsHolder dummySettingsHolder)
        {
            _dummySettingsHolder = dummySettingsHolder ?? throw new ArgumentNullException(nameof(dummySettingsHolder));
        }

        [HttpGet]
        [SwaggerOperation("GetPercentage")]
        [ProducesResponseType(typeof(BankCardsFeeResponseModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public IActionResult GetPercentage()
        {
            return Ok(new BankCardsFeeResponseModel
                {Percentage = _dummySettingsHolder.GetBankCardSettings().PercentageFeeSize});
        }
    }
}
