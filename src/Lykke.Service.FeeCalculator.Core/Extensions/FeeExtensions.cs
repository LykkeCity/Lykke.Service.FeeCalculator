using System;
using System.IO;
using System.Text;
using Lykke.Service.FeeCalculator.Core.Domain.Fees;
using MessagePack;

namespace Lykke.Service.FeeCalculator.Core.Extensions
{
    public static class FeeExtensions
    {
        public static byte[] SerializeFee(this IFeeId fee, Func<IFeeId, object> getCachedFee)
        {
            var idBytes = Encoding.ASCII.GetBytes(fee.Id);
            
            using (var stream = new MemoryStream())
            {
                stream.Write(idBytes, 0, idBytes.Length);
                var cachedFee = getCachedFee(fee);
                MessagePackSerializer.Serialize(stream, cachedFee);

                stream.Flush();

                return stream.ToArray();
            }
        }
        
        public static T DeserializeFee<T>(this byte[] value) where T: class
        {
            var idLength = Guid.Empty.ToString().Length;
            
            using (var stream = new MemoryStream(value, idLength, value.Length - idLength, writable: false))
            {
                return MessagePackSerializer.Deserialize<T>(stream);
            }
        }
    }
}
