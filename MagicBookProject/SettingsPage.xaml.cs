using MagicBookProject.ViewModel;
using Plugin.Maui.Audio;
namespace MagicBookProject;

public partial class SettingsPage : ContentPage
{
	private IAudioPlayer player;
	public SettingsPage(InfoViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
	private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
	{
		MainPage.player.Volume = e.NewValue / 100.0;
	}
}