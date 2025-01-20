using AppGuayaquil.Domain.Ports;
using AppGuayaquil.Infrastructure.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace AppGuayaquil.Infrastructure.Extensions;

public static class AutoLoadServices
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<ICredentialsRepository, CredentialsRepository>();
        services.AddTransient<IPeopleRepository, PeopleRepository>();
       
        return services;
    }

}
