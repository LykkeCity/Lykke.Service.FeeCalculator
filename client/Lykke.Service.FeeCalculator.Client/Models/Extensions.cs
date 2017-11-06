using Lykke.Service.FeeCalculator.Client.AutorestClient.Models;

namespace Lykke.Service.FeeCalculator.Client.Models
{
    public static class Extensions
    {
        public static TradeFeeModel FromApiModel(this FeeResponse model)
        {
            return new TradeFeeModel
            {
                Value = (decimal) model.Value
            };
        }
    }
}
