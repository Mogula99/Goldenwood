using GoldenwoodClient.ExternalApis;
using GoldenwoodClient.Infrastructure;
using GoldenwoodClient.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Refit;
using System.Reflection;

namespace GoldenwoodClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.appsettings.json");
            var configurationBuilder = new ConfigurationBuilder().AddJsonStream(stream);
            var configuration = configurationBuilder.Build();
            builder.Services.AddSingleton<IConfiguration>(configuration);

            var baseUrl = configuration["BackendApiBaseUrl"];
            var militaryApi = RestService.For<IMilitaryApi>(baseUrl);
            builder.Services.AddSingleton(militaryApi);

            var resourcesApi = RestService.For<IResourcesApi>(baseUrl);
            builder.Services.AddSingleton(resourcesApi);

            builder.Services.AddSingleton<UnitGroupConverter>();
            builder.Services.AddSingleton<ResourcesRecordConverter>();



            builder.Services.AddTransient<VillagePage>();
            builder.Services.AddTransient<VillageVm>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainVm>();

            builder.Services.AddSingleton<MapPage>();
            builder.Services.AddSingleton<MapVm>();
#endif

            return builder.Build();
        }
    }
}
