using MagicBookProject.ViewModel;

namespace MagicBookProject;

public partial class InfoPage : ContentPage
{
	public InfoPage(InfoViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    int count = 0;
    private async void Flashlight_party(object sender, EventArgs e)
    {
        Vibration.Vibrate(50);
        count++;
        if (count >= 5)
        {
            for(int i=0; i<10; i++)
            {
                await Flashlight.TurnOnAsync();
                party.BorderColor = Colors.Orange;
                party.TextColor = Colors.Orange;
                await Task.Delay(50);
                await Flashlight.TurnOffAsync();
                party.BorderColor = Colors.Brown;
                party.TextColor = Colors.DarkRed;
            }
            count = 0;
        }
    }
}