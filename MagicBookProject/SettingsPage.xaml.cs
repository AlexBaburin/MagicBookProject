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
        Choice(MainViewModel.str_speed);
	}
    public async void WriteTextToFile(string text, string targetFileName)
    {
        // Write the file content to the app data directory  
        string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, targetFileName);
        FileStream fileStream = File.Open(targetFile, FileMode.Open);
        // Write the file content to the app data directory 
        fileStream.SetLength(0);
        fileStream.Close();

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
                PlayPage.SpeedText = 5;
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
                    PlayPage.SpeedText = 5;
                    break;
            }
        string mainDir = FileSystem.Current.AppDataDirectory;
        if (!File.Exists($"{mainDir}/speed.txt"))
            File.Create($"{mainDir}/speed.txt").Close();
        WriteTextToFile((string)radioButton.Content, "speed.txt");
    }

    private async void resetButtonClicked(object sender, EventArgs e)
    {
        button_reset.BorderColor = Colors.Orange;
        button_reset.TextColor = Colors.Orange;
        Vibration.Vibrate(50);
        WriteTextToFile("0", "save.txt");
        await Task.Delay(100);
        button_reset.BorderColor = Colors.Firebrick;
        button_reset.TextColor=Colors.DarkRed;
    }
    int Denis_count = 1;
    private async void DenisClicked(object sender, EventArgs e)
    {
        Vibration.Vibrate(50);
        Denis_count++;
        if (Denis_count % 2 == 0)
        {

            await Flashlight.TurnOnAsync();
            Lagutin.BorderColor = Colors.Orange;
            Lagutin.TextColor = Colors.Orange;
        }
        else
        {
            await Flashlight.TurnOffAsync();
            Lagutin.BorderColor = Colors.Brown;
            Lagutin.TextColor = Colors.DarkRed;
        }
    }
}