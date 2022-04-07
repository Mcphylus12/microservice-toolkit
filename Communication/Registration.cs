using Communication.Abstractions.Registration;
using Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Communication;

public static class Registration
{
    public static void RegisterCommunication(this IServiceCollection services)
    {
        Abstractions.Registration.Registration.RegisterCommunicationAbstractions(services);

        services.AddHttpClient();
        services.AddTransient<IMessageImplementation, RabbitMQImplementation>();
        services.AddTransient<IRequestImplementation, HttpImplementation>();
        services.AddTransient<ISender, Sender>();

        services.SetupOptions<InboundMessagingConfig>()
            .WithBinder<InboundMessagingConfigurationBinder>();

        services.SetupOptions<OutboundMessagingConfig>()
            .WithBinder<OutboundMessagingConfigurationBinder>();

        services.SetupOptions<OutboundRequestConfig>()
            .WithBinder<OutboundRequestConfigurationBinder>();
    }

    public static void HandleMessages(this IServiceCollection services)
    {
        services.AddHostedService<HostedMessageService>();
    }

    public static void HandleRequests(this IServiceCollection services)
    {
        services.AddTransient<RequestMiddleware>();
    }

    public static void HandleRequests(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestMiddleware>();
    }
}
