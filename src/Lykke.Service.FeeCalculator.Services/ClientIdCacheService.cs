using System.Collections.Concurrent;
using System.Threading.Tasks;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ClientAccount.Client.AutorestClient.Models;
using Lykke.Service.FeeCalculator.Core.Services;

namespace Lykke.Service.FeeCalculator.Services
{
    public class ClientIdCacheService : IClientIdCacheService
    {
        private readonly IClientAccountClient _clientAccountClient;
        private readonly ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();

        public ClientIdCacheService(IClientAccountClient clientAccountClient)
        {
            _clientAccountClient = clientAccountClient;
        }
        
        public async Task<string> GetClientId(string id)
        {
            if (_cache.ContainsKey(id))
                return _cache[id];

            var client = await _clientAccountClient.GetByIdAsync(id);

            if (client != null)
            {
                _cache[id] = client.Id;
                return client.Id;
            }

            var wallet = await _clientAccountClient.GetWalletAsync(id);

            if (wallet == null) 
                return id;
            
            string clientId = wallet.Type == WalletType.Trading.ToString()
                ? wallet.ClientId
                : wallet.Id;

            _cache[id] = clientId;
            return clientId;
        }
    }
}
