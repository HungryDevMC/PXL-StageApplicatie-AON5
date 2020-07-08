using Microsoft.Extensions.DependencyInjection;
using Stage_API.Business;
using Stage_API.Business.Abstractions;

namespace Stage_API.Extensions
{
    public static class Ioc
    {
        internal static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IInternshipService, InternshipService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
