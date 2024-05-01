using MagicBookProject.ViewModel;

namespace MagicBookProject;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(InfoViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}