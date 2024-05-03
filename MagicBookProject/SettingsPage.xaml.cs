using MagicBookProject.ViewModel;
namespace MagicBookProject;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(InfoViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
	private void Slider_ValueChanged(object sender, EventArgs e)
	{
		
	}
}