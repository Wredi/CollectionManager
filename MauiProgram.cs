using CollectionManager.Data;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CollectionManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif
            string dbPath = FileSystem.AppDataDirectory;
            builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<CollectionRepository>(s, dbPath));
            Trace.WriteLine(dbPath);

            //builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
            //builder.Services.AddTransient<ImportAndExportPage>();

            return builder.Build();
        }
    }
}