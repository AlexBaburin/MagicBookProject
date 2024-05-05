using MagicBookProject.ViewModel;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;

namespace MagicBookProject
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
            string targetFileSave = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "save.txt");
            if (!File.Exists(targetFileSave))
                File.Create(targetFileSave).Close();
            string targetFileSettings = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "settings.txt");
            if (!File.Exists(targetFileSettings))
                File.Create(targetFileSettings).Close();
            string targetFileSpeed = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "speed.txt");
            if (!File.Exists(targetFileSpeed))
                File.Create(targetFileSpeed).Close();
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<MainPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<InfoPage>();
            builder.Services.AddTransient<InfoViewModel>();

            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsViewModel>();

            builder.Services.AddTransient<PlayPage>();
            builder.Services.AddTransient<PlayViewModel>();

            return builder.Build();
        }
    }
}
