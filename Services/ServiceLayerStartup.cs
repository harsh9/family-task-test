using Core.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Services
{
    public static class ServiceLayerStartup
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {
            services.AddTransient<IMemberService, MemberService>();
            services.AddTransient<ITaskService, TaskService>();
        }
    }
}