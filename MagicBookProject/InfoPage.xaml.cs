using MagicBookProject.ViewModel;

namespace MagicBookProject;

public partial class InfoPage : ContentPage
{
	public InfoPage(InfoViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}