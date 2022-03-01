using Communication.Abstractions.Registration;
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

    public static OptionsSetup<TOptions> SetupOptions<TOptions>(this IServiceCollection services)
        where TOptions : class
    {
        return new OptionsSetup<TOptions>(services);
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

public class OptionsSetup<TOptions>
    where TOptions : class
{
    public OptionsBuilder<TOptions> Builder { get; }

    private readonly IServiceCollection _services;

    public OptionsSetup(IServiceCollection services)
    {
        Builder = new OptionsBuilder<TOptions>(services, null);
        _services = services;
    }

    internal OptionsSetup<TOptions> WithBinder<TBinder>()
        where TBinder : class, IConfigureOptions<TOptions>, IValidateOptions<TOptions>
    {
        _services.AddTransient<IConfigureOptions<TOptions>, TBinder>();
        _services.AddTransient<IValidateOptions<TOptions>, TBinder>();
        return this;
    }
}
