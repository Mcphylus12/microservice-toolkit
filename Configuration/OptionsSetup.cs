using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Configuration;

public static class Extensions
{
    public static OptionsSetup<TOptions> SetupOptions<TOptions>(this IServiceCollection services)
        where TOptions : class
    {
        return new OptionsSetup<TOptions>(services);
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

    public OptionsSetup<TOptions> WithBinder<TBinder>()
        where TBinder : class, IConfigureOptions<TOptions>, IValidateOptions<TOptions>
    {
        _services.AddTransient<IConfigureOptions<TOptions>, TBinder>();
        _services.AddTransient<IValidateOptions<TOptions>, TBinder>();
        return this;
    }
}
