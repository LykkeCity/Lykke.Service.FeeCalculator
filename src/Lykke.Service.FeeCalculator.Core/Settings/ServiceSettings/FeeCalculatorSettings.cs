using System;
using System.Collections.Generic;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.SettingsReader.Attributes;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;
using Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FeeCalculator.Core.Settings.ServiceSettings
{
    public class FeeCalculatorSettings
    {
        public int TradeVolumeToGetInDays { get; set; }
        public CacheSettings Cache { get; set; }
        public DbSettings Db { get; set; }
        public IReadOnlyCollection<CashoutFee> CashoutFees { get; set; }
        public IReadOnlyCollection<MarketOrderAssetFee> MarketOrderFees { get; set; }
        public IReadOnlyCollection<WithdrawalFee> WithdrawalFees { get; set; }
        public BankCardSettings BankCard { get; set; }
    }
}
