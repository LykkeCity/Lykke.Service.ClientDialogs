using System;
using Autofac;

namespace Lykke.Service.ClientDialogs.Client
{
    /// <summary>
    /// Autofac extension to register client dialogs client
    /// </summary>
    public static class AutofacExtension
    {
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

        public static void RegisterClientDialogsClient(this ContainerBuilder builder, ClientDialogsServiceClientSettings settings)
        {
            builder.RegisterClientDialogsClient(settings?.ServiceUrl);
        }
    }
}
