﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Lykke.Service.FeeCalculator.Core.Services;
using Lykke.Service.FeeCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FeeCalculator.Controllers
{
    [Route("api/Fee")]
    public class FeeController : Controller
    {
        private readonly IFeeService _feeService;

        public FeeController(
            IFeeService feesCache
            )
        {
            _feeService = feesCache;
        }

        /// <summary>
        /// Adds a dynamic fee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [SwaggerOperation("AddFee")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddFee([FromBody]FeeModel model)
        {
            if (!string.IsNullOrEmpty(model.Id) && !model.Id.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Id)} value"));
            
            var fees = await _feeService.GetAllAsync();

            if (fees.Any(item => item.Volume == model.Volume && item.Id != model.Id))
                return BadRequest($"fee for volume '{model.Volume}' is already added");
            
            await _feeService.AddAsync(new Core.Domain.Fees.Fee
            {
                Id = model.Id,
                Volume = model.Volume,
                MakerFee = model.MakerFee,
                TakerFee = model.TakerFee,
                MakerFeeType = model.MakerFeeType,
                TakerFeeType = model.TakerFeeType,
                MakerFeeModificator = model.MakerFeeModificator
            });
            
            return Ok();
        }
        
        /// <summary>
        /// Gets all the dynamic fees
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        [SwaggerOperation("GetFees")]
        [ProducesResponseType(typeof(List<Fee>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetFees()
        {
            var fees = await _feeService.GetAllAsync();
            var result = Mapper.Map<List<Fee>>(fees);
            return Ok(result);
        }
        
        /// <summary>
        /// Deletes the dynamic fee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("delete/{id}")]
        [SwaggerOperation("DeleteFee")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteFee(string id)
        {
            if (!id.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(id)} value"));
            
            await _feeService.DeleteAsync(id);
            return Ok();
        }
    }
}
