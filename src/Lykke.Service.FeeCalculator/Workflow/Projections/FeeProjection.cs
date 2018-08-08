using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Service.FeeCalculator.Core.Domain.FeeLog;
using Lykke.Service.PostProcessing.Contracts.Cqrs.Events;

namespace Lykke.Service.FeeCalculator.Workflow.Projections
{
    public class FeeProjection
    {
        private readonly IFeeLogRepository _feelogRepository;
        private readonly IChaosKitty _chaosKitty;

        public FeeProjection([NotNull] IFeeLogRepository feelogRepository, [NotNull] IChaosKitty chaosKitty)
        {
            _feelogRepository = feelogRepository ?? throw new ArgumentNullException(nameof(feelogRepository));
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
        }

        public async Task Handle(FeeChargedEvent evt)
        {
            var newItem = new FeeLogEntry
            {
                OperationId = evt.OperationId,
                Type = (FeeOperationType)(int)evt.OperationType,
                Fee = evt.Fee
            };

            await _feelogRepository.CreateAsync(newItem);

            _chaosKitty.Meow("DB problem");
        }

    }
}
