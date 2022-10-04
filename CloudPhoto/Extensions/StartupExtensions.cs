using CloudPhoto.Helpers;
using CloudPhoto.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudPhoto.Extensions
{
    public static class StartupExtensions
    {
        public static T GetSettings<T>(this IConfiguration configuration, string name) where T : new()
        {
            var result = new T();
            configuration.GetSection(name).Bind(result);
            return result;
        }

        public static void AddAws(this IServiceCollection services)
        {
            var cloudSettings = CloudSettings.FromIni(Constants.ConfigPath);
            services.AddSingleton(cloudSettings);

            var client = AwsHelper.CreateClient(cloudSettings);
            services.AddSingleton(client);
        }
    }
}
