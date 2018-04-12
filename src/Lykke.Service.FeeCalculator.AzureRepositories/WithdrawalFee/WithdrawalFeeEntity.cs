using System.Collections.Generic;
using Common;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee;

namespace Lykke.Service.FeeCalculator.AzureRepositories.WithdrawalFee
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class WithdrawalFeeEntity : AzureTableEntity, IWithdrawalFee
    {
        public string AssetId { get; set; }
        public double Size { get; set; }
        public PaymentSystemType PaymentSystem { get; set; }
        public string SerializedCountries { get; set; }

        public IReadOnlyCollection<string> Countries {
            get
            {
                return SerializedCountries.DeserializeJson<IReadOnlyCollection<string>>();
            }
            set
            {
                SerializedCountries = value.ToJson();
            }
        }



        internal static string GeneratePartitionKey() => "WithdrawalFee";
        internal static string GenerateRowKey(string id) => id;


        internal static WithdrawalFeeEntity Create(IWithdrawalFee fee)
        {
            return new WithdrawalFeeEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(fee.AssetId),
                AssetId = fee.AssetId,
                Size = fee.Size,
                Countries = fee.Countries
            };
        }
    }
}
