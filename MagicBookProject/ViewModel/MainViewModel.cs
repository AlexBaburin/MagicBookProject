using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Data;
using System.IO;

namespace MagicBookProject.ViewModel;

public partial class MainViewModel : ObservableObject
{
    public static double value;
    public static string str_speed;

    [RelayCommand]
    async Task TapInfo()
    {
        await Shell.Current.GoToAsync(nameof(InfoPage));
        Vibration.Vibrate(100);
    }

    public async Task<string> ReadTextFile(string targetFileName)
    {
        targetFileName = targetFileName.Remove(0, 1);
        string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, targetFileName);
        using FileStream InputStream = System.IO.File.OpenRead(targetFile);
        using StreamReader reader = new StreamReader(InputStream);
        string? line = await reader.ReadLineAsync();
        if (line == "")
            line = "0";
        if (line == "Быстраяая")
            line = "Быстрая";
        string line2 = line;
        return line2;
    }
    public async void WriteTextToFile(string text, string targetFileName)
    {
        string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, targetFileName);
        FileStream fileStream = File.Open(targetFile, FileMode.Open);
        // Write the file content to the app data directory 
        fileStream.SetLength(0);
        fileStream.Close();
        
        using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
        using StreamWriter streamWriter = new StreamWriter(outputStream);
        await streamWriter.WriteAsync(text);
    }
    [RelayCommand]
    async Task TapSettings()
    {
        string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "settings.txt");
        if (!File.Exists(targetFile))
        {
            File.Create(targetFile).Close();
            WriteTextToFile("50", "settings.txt");
        }
        string line = await ReadTextFile("/settings.txt");
        value = Convert.ToDouble(line);

        targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "speed.txt");
        //File.Delete(targetFile);
        if (!File.Exists(targetFile))
        {
            File.Create(targetFile).Close();
            WriteTextToFile("Обычная", "speed.txt");
        }
        line = await ReadTextFile("/speed.txt");
        str_speed = line;
        await Shell.Current.GoToAsync(nameof(SettingsPage));
        Vibration.Vibrate(100);
    }
    [RelayCommand]
    async Task TapPlay()
    {
        string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "save.txt");
        if (!File.Exists(targetFile))
        {
            File.Create(targetFile).Close();
            WriteTextToFile("0", "save.txt");
        }
        string line = await ReadTextFile("/save.txt");
        PlayPage.storyIndex = Convert.ToInt32(line);
        await Shell.Current.GoToAsync(nameof(PlayPage));
        Vibration.Vibrate(100);
    }
}
