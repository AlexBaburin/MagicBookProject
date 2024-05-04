using MagicBookProject.ViewModel;
using Plugin.Maui.Audio;
using static Java.Util.Jar.Attributes;
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
        Choice(MainViewModel.str_speed);
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
    private void Choice(string name)
    {
        RadioButton radioButton;
        switch (name)
        {
            case "Быстрая":
                PlayPage.SpeedText = 0.05;
                radioButton = (RadioButton)FindByName("radiobutton1");
                radioButton.IsChecked = true;
                break;
            case "Обычная":
                PlayPage.SpeedText = 0.25;
                radioButton = (RadioButton)FindByName("radiobutton2");
                radioButton.IsChecked = true;
                break;
            case "Медленная":
                PlayPage.SpeedText = 1;
                radioButton = (RadioButton)FindByName("radiobutton3");
                radioButton.IsChecked = true;
                break;
        }
    }
    private void SpeedText(object sender, CheckedChangedEventArgs e)
    {
        RadioButton radioButton = (RadioButton)sender;
        if (radioButton.IsChecked)
            switch (radioButton.Content)
            {
                case "Быстрая":
                    PlayPage.SpeedText = 0.05;
                    break;
                case "Обычная":
                    PlayPage.SpeedText = 0.25;
                    break;
                case "Медленная":
                    PlayPage.SpeedText = 1;
                    break;
            }
        string mainDir = FileSystem.Current.AppDataDirectory;
        if (!File.Exists($"{mainDir}/speed.txt"))
            File.Create($"{mainDir}/speed.txt").Close();
        WriteTextToFile((string)radioButton.Content, "speed.txt");
    }
  
}