using MagicBookProject.ViewModel;

namespace MagicBookProject;

public partial class PlayPage : ContentPage
{
	public PlayPage(PlayViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}