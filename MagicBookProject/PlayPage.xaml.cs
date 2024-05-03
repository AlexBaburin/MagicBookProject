using MagicBookProject.ViewModel;

namespace MagicBookProject;

public partial class PlayPage : ContentPage
{
	public PlayPage(PlayViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
	private async void AnimatedText(Label text)
	{
		string downloadText = "Я тот самый человек, который лицезрел мирозданиe во время величия хаоса на Земле";
		text.Text = string.Empty;
		foreach(char c in downloadText)
		{
			text.Text += c;
			await Task.Delay(100);
		}
	}
	private void OnImageTapped(object sender, EventArgs e)
	{
		if (sender is Image image)
		{
			var text = (Label)image.Parent.FindByName("MainText");
			AnimatedText(text);
		}
	}
}