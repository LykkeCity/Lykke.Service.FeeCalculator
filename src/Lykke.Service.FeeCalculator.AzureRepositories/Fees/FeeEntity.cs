using System.Globalization;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;

namespace Lykke.Service.FeeCalculator.AzureRepositories.Fees
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class FeeEntity : AzureTableEntity, IFee
    {
        public decimal Volume { get; set; }
        public decimal TakerFee { get; set; }
        public decimal MakerFee { get; set; }

        internal static string GeneratePartitionKey() => "Fee";
        internal static string GenerateRowKey(decimal volume) => volume.ToString(CultureInfo.InvariantCulture);

        public static FeeEntity Create(IFee fee)
        {
            return new FeeEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(fee.Volume),
                Volume = fee.Volume,
                MakerFee = fee.MakerFee,
                TakerFee = fee.TakerFee
            };
        }
    }
}
