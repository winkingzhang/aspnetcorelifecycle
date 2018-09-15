using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _03___mvc.Services
{
    public interface ITrackingServer
    {
        Guid ServerGuid { get; }
    }

    public class TrackingServerService : ITrackingServer
    {
        public Guid ServerGuid { get; } = Guid.NewGuid();
    }

    public static class ServicesExtensions
    {
        public static void AddTrackingServer(this IServiceCollection services)
        {
            services.AddSingleton<ITrackingServer>(new TrackingServerService());
        }
    }
}
