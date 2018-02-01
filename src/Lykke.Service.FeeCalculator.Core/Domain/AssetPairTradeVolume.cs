namespace Lykke.Service.FeeCalculator.Core.Domain
{
    public class AssetPairTradeVolume
    {
        public string BaseAssetId { get; set; }
        public string QuoteAssetId { get; set; }
        public double BaseVolume { get; set; }
        public double QuotingVolume { get; set; }
    }
}
