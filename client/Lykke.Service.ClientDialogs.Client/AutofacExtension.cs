using System;
using Autofac;
using JetBrains.Annotations;
using Lykke.HttpClientGenerator.Infrastructure;

namespace Lykke.Service.ClientDialogs.Client
{
    /// <summary>
    /// Autofac extension to register client dialogs client
    /// </summary>
    [PublicAPI]
    public static class AutofacExtension
    {
        /// <summary>
        /// Registers client dialogs client
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceUrl"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void RegisterClientDialogsClient(this ContainerBuilder builder, string serviceUrl)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterInstance(
                    new ClientDialogsClient(HttpClientGenerator.HttpClientGenerator.BuildForUrl(serviceUrl)
                        .WithAdditionalCallsWrapper(new ExceptionHandlerCallsWrapper())
                        .WithoutRetries()
                        .Create())
                )
                .As<IClientDialogsClient>()
                .SingleInstance();
        }

        /// <summary>
        /// Registers client dialogs client
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="settings"></param>
        public static void RegisterClientDialogsClient(this ContainerBuilder builder, ClientDialogsServiceClientSettings settings)
        {
            builder.RegisterClientDialogsClient(settings?.ServiceUrl);
        }
    }
}
