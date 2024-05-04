using MagicBookProject.ViewModel;
using Plugin.Maui.Audio;
namespace MagicBookProject;

public partial class SettingsPage : ContentPage
{
	private IAudioPlayer player;
	public static string volume = "1";
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        Volume.Value = MainViewModel.value;
	}
    public async void WriteTextToFile(string text, string targetFileName)
    {
        // Write the file content to the app data directory  
        string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, targetFileName);
        using FileStream outputStream = System.IO.File.OpenWrite(targetFile);
        using StreamWriter streamWriter = new StreamWriter(outputStream);
        await streamWriter.WriteAsync(text);
    }
    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
	{
		volume = Convert.ToString(Convert.ToDecimal(e.NewValue));
        MainPage.player.Volume = e.NewValue / 100.0;

        string mainDir = FileSystem.Current.AppDataDirectory;
        if (!File.Exists($"{mainDir}/settings.txt"))
            File.Create($"{mainDir}/settings.txt").Close();
        WriteTextToFile(volume, "settings.txt");
    }
  
}