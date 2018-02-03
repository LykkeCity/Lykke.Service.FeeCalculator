// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.FeeCalculator.AutorestClient
{
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for FeeCalculatorAPI.
    /// </summary>
    public static partial class FeeCalculatorAPIExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='assetId'>
            /// </param>
            public static object GetCashoutFees(this IFeeCalculatorAPI operations, string assetId = default(string))
            {
                return operations.GetCashoutFeesAsync(assetId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='assetId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetCashoutFeesAsync(this IFeeCalculatorAPI operations, string assetId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetCashoutFeesWithHttpMessagesAsync(assetId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object AddFee(this IFeeCalculatorAPI operations, FeeModel model = default(FeeModel))
            {
                return operations.AddFeeAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> AddFeeAsync(this IFeeCalculatorAPI operations, FeeModel model = default(FeeModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddFeeWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object GetFees(this IFeeCalculatorAPI operations)
            {
                return operations.GetFeesAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetFeesAsync(this IFeeCalculatorAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetFeesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='volume'>
            /// </param>
            public static object DeleteFee(this IFeeCalculatorAPI operations, decimal volume)
            {
                return operations.DeleteFeeAsync(volume).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='volume'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteFeeAsync(this IFeeCalculatorAPI operations, decimal volume, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteFeeWithHttpMessagesAsync(volume, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object AddStaticFee(this IFeeCalculatorAPI operations, StaticFeeModel model = default(StaticFeeModel))
            {
                return operations.AddStaticFeeAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> AddStaticFeeAsync(this IFeeCalculatorAPI operations, StaticFeeModel model = default(StaticFeeModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddStaticFeeWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object GetStaticFees(this IFeeCalculatorAPI operations)
            {
                return operations.GetStaticFeesAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetStaticFeesAsync(this IFeeCalculatorAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetStaticFeesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='assetPair'>
            /// </param>
            public static object DeleteStaticFee(this IFeeCalculatorAPI operations, string assetPair)
            {
                return operations.DeleteStaticFeeAsync(assetPair).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='assetPair'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteStaticFeeAsync(this IFeeCalculatorAPI operations, string assetPair, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteStaticFeeWithHttpMessagesAsync(assetPair, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Checks service is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object IsAlive(this IFeeCalculatorAPI operations)
            {
                return operations.IsAliveAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Checks service is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> IsAliveAsync(this IFeeCalculatorAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.IsAliveWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='orderAction'>
            /// Possible values include: 'Buy', 'Sell'
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='assetPair'>
            /// </param>
            /// <param name='assetId'>
            /// </param>
            public static object GetMarketOrderFee(this IFeeCalculatorAPI operations, OrderAction orderAction, string clientId = default(string), string assetPair = default(string), string assetId = default(string))
            {
                return operations.GetMarketOrderFeeAsync(orderAction, clientId, assetPair, assetId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='orderAction'>
            /// Possible values include: 'Buy', 'Sell'
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='assetPair'>
            /// </param>
            /// <param name='assetId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetMarketOrderFeeAsync(this IFeeCalculatorAPI operations, OrderAction orderAction, string clientId = default(string), string assetPair = default(string), string assetId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetMarketOrderFeeWithHttpMessagesAsync(orderAction, clientId, assetPair, assetId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='orderAction'>
            /// Possible values include: 'Buy', 'Sell'
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='assetPair'>
            /// </param>
            /// <param name='assetId'>
            /// </param>
            public static object GetLimitOrderFee(this IFeeCalculatorAPI operations, OrderAction orderAction, string clientId = default(string), string assetPair = default(string), string assetId = default(string))
            {
                return operations.GetLimitOrderFeeAsync(orderAction, clientId, assetPair, assetId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='orderAction'>
            /// Possible values include: 'Buy', 'Sell'
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='assetPair'>
            /// </param>
            /// <param name='assetId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetLimitOrderFeeAsync(this IFeeCalculatorAPI operations, OrderAction orderAction, string clientId = default(string), string assetPair = default(string), string assetId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetLimitOrderFeeWithHttpMessagesAsync(orderAction, clientId, assetPair, assetId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
