using System;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FeeCalculator.Client.AutorestClient;
using Lykke.Service.FeeCalculator.Client.AutorestClient.Models;
using Lykke.Service.FeeCalculator.Client.Models;

namespace Lykke.Service.FeeCalculator.Client
{
    public class FeeCalculatorClient : IFeeCalculatorClient, IDisposable
    {
        private readonly ILog _log;
        private IFeeCalculatorAPI _service;

        public FeeCalculatorClient(string serviceUrl, ILog log)
        {
            _log = log;
            _service = new FeeCalculatorAPI(new Uri(serviceUrl));
        }

        public void Dispose()
        {
            if (_service == null)
                return;
            _service.Dispose();
            _service = null;
        }

        public async Task<TradeFeeModel> GetTradeFees(string clientId, string assetPair, string assetId, OrderAction orderAction)
        {
            var response = await _service.GetTradeFeeWithHttpMessagesAsync(clientId, assetPair, assetId, orderAction);

            var error = response.Body as ErrorResponse;

            if (response.Body is FeeResponse result)
            {
                return result.FromApiModel();
            }

            if (error != null)
            {
                await _log.WriteErrorAsync(GetType().Name, "GetTradeFee",
                    $"clientId = {clientId}, assetPair = {assetPair}, assetId = {assetId}, orderAction = {orderAction}, error = {error.ErrorMessage}", null);
            }

            return null;
        }
    }
}
