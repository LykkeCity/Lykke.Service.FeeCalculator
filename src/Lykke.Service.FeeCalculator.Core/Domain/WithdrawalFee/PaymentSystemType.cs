using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.FeeCalculator.Core.Domain.WithdrawalFee
{
    public enum PaymentSystemType
    {
        Unknown,
        Sepa,
        Swift,
        Swiss
    }
}
