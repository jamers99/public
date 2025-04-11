using Microsoft.Extensions.DependencyInjection;

namespace OfflineTasks.MyEbms;

public static class DependencyInjection
{
    public static IServiceCollection AddMyEbms(this IServiceCollection services, Action<ODataConnection> odata) => services
        .Configure(odata)
        .AddSingleton<IMyEbmsAuthentication, MyEbmsOdataAuthentication>()
        .AddSingleton<MyEbmsOdata>();
}
