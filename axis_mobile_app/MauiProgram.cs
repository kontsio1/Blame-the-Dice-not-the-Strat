using Microsoft.Extensions.Logging;

namespace axis_mobile_app;

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
#endif

        builder.Services.AddSingleton<MainPageModel>();
        builder.Services.AddSingleton<NavalPageModel>();
        builder.Services.AddSingleton<CounterPageModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<ResultsPage>();
        builder.Services.AddSingleton<NavalMainPage>();
        builder.Services.AddSingleton<NavalResultsPage>();
        builder.Services.AddSingleton<CounterMainPage>();
        builder.Services.AddSingleton<CounterResultsPage>();
        builder.Services.AddSingleton<AboutPage>();

        return builder.Build();
    }
}