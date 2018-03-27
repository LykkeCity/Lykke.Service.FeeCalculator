using AutoMapper;
using Lykke.Service.FeeCalculator.Core.Domain.CashoutFee;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using Lykke.Service.FeeCalculator.Core.Domain.MarketOrderAssetFee;

namespace Lykke.Service.FeeCalculator.Profiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<IFee, Models.Fee>(MemberList.Source);
            CreateMap<ICashoutFee, Models.CashoutFee>(MemberList.Source);
            CreateMap<IMarketOrderAssetFee, Models.MarketOrderAssetFee>(MemberList.Source);
            CreateMap<IStaticFee, Models.StaticFee>(MemberList.Source);
        }
    }
}
