using CloudPhoto.Extensions;
using CloudPhoto.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CloudPhoto
{
    public static class Startup
    {
        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var configuration = context.Configuration;
            services.AddSingleton(configuration.GetSettings<AppSettings>("AppSettings"));
            services.AddAws();
        }
    }
}
