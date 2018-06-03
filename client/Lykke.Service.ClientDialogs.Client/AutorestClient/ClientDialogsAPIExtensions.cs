// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.ClientDialogs.AutorestClient
{
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for ClientDialogsAPI.
    /// </summary>
    public static partial class ClientDialogsAPIExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            public static object GetClientDialogs(this IClientDialogsAPI operations, string clientId)
            {
                return operations.GetClientDialogsAsync(clientId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetClientDialogsAsync(this IClientDialogsAPI operations, string clientId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetClientDialogsWithHttpMessagesAsync(clientId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='dialogId'>
            /// </param>
            /// <param name='clientId'>
            /// </param>
            public static object GetClientDialog(this IClientDialogsAPI operations, string dialogId, string clientId = default(string))
            {
                return operations.GetClientDialogAsync(dialogId, clientId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='dialogId'>
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetClientDialogAsync(this IClientDialogsAPI operations, string dialogId, string clientId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetClientDialogWithHttpMessagesAsync(dialogId, clientId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object GetCommonDialogs(this IClientDialogsAPI operations)
            {
                return operations.GetCommonDialogsAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetCommonDialogsAsync(this IClientDialogsAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetCommonDialogsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            public static object GetSubmittedDialogs(this IClientDialogsAPI operations, string clientId)
            {
                return operations.GetSubmittedDialogsAsync(clientId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='clientId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetSubmittedDialogsAsync(this IClientDialogsAPI operations, string clientId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetSubmittedDialogsWithHttpMessagesAsync(clientId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static ErrorResponse AddClientDialog(this IClientDialogsAPI operations, ClientDialogModel model = default(ClientDialogModel))
            {
                return operations.AddClientDialogAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> AddClientDialogAsync(this IClientDialogsAPI operations, ClientDialogModel model = default(ClientDialogModel), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddClientDialogWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static ErrorResponse DeleteClientDialog(this IClientDialogsAPI operations, DeleteDialogRequest request = default(DeleteDialogRequest))
            {
                return operations.DeleteClientDialogAsync(request).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> DeleteClientDialogAsync(this IClientDialogsAPI operations, DeleteDialogRequest request = default(DeleteDialogRequest), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteClientDialogWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static ErrorResponse SubmitDialog(this IClientDialogsAPI operations, SubmitDialogRequest request = default(SubmitDialogRequest))
            {
                return operations.SubmitDialogAsync(request).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ErrorResponse> SubmitDialogAsync(this IClientDialogsAPI operations, SubmitDialogRequest request = default(SubmitDialogRequest), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.SubmitDialogWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            public static object IsDialogSubmitted(this IClientDialogsAPI operations, SubmitDialogRequest request = default(SubmitDialogRequest))
            {
                return operations.IsDialogSubmittedAsync(request).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='request'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> IsDialogSubmittedAsync(this IClientDialogsAPI operations, SubmitDialogRequest request = default(SubmitDialogRequest), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.IsDialogSubmittedWithHttpMessagesAsync(request, null, cancellationToken).ConfigureAwait(false))
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
            public static IsAliveResponse IsAlive(this IClientDialogsAPI operations)
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
            public static async Task<IsAliveResponse> IsAliveAsync(this IClientDialogsAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.IsAliveWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
