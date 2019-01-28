namespace Lykke.Service.FeeCalculator.Services.DummySettingsHolder
{
    public class DummySettingsHolder : IDummySettingsHolder
    {
        private readonly double _percentageFeeSize;

        public DummySettingsHolder(double percentageFeeSize)
        {
            _percentageFeeSize = percentageFeeSize;
        }

        public double GetPercentageFeeSize()
        {
            return _percentageFeeSize;
        }
    }
}
