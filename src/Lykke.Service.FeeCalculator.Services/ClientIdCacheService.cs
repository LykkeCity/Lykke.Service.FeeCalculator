using System.Collections.Concurrent;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.FeeCalculator.Core.Services;

namespace Lykke.Service.FeeCalculator.Services
{
    public class ClientIdCacheService : IClientIdCacheService
    {
        private readonly IClientAccountClient _clientAccountClient;
        private readonly ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();

        [UsedImplicitly]
        public ClientIdCacheService(IClientAccountClient clientAccountClient)
        {
            _clientAccountClient = clientAccountClient;
        }
        
        public async Task<string> GetClientId(string id)
        {
            if (_cache.ContainsKey(id))
                return _cache[id];

            try
            {
                var client = await _clientAccountClient.GetByIdAsync(id);

                if (client != null)
                {
                    _cache[id] = client.Id;
                    return client.Id;
                }
            }
            catch{}

            try
            {
                var wallet = await _clientAccountClient.GetWalletAsync(id);

                if (wallet == null)
                    return id;

                _cache[id] = wallet.ClientId;
                return wallet.ClientId;
            }
            catch
            {
                return id;
            }
        }
    }
}
