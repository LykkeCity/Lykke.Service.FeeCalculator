using System.Collections.Generic;
using Common;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.FeeCalculator.AzureRepositories.WithdrawalFee
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class WithdrawalFeeEntity : AzureTableEntity, IWithdrawalFeeModel
    {
        public string AssetId { get; set; }
        public double SizeForSelectedCountries { get; set; }
        public double SizeForOtherCountries { get; set; }
        public PaymentSystemType PaymentSystemForSelectedCountries { get; set; }
        public PaymentSystemType PaymentSystemForOtherCountries { get; set; }

        [IgnoreProperty]
        public IReadOnlyCollection<string> Countries { get; set; }

        public string SerializedCountries { get; set; }


        internal static string GeneratePartitionKey() => "WithdrawalFee";
        internal static string GenerateRowKey(string id) => id;


        internal static WithdrawalFeeEntity Create(WithdrawalFeeModel fee)
        {
            return new WithdrawalFeeEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(fee.AssetId),
                AssetId = fee.AssetId,
                SizeForSelectedCountries = fee.SizeForSelectedCountries,
                PaymentSystemForSelectedCountries = fee.PaymentSystemForSelectedCountries,
                PaymentSystemForOtherCountries= fee.PaymentSystemForOtherCountries,
                SizeForOtherCountries = fee.SizeForOtherCountries,
                SerializedCountries = fee.Countries.ToJson()
            };
        }
    }
}
