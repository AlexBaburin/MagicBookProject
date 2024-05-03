using MagicBookProject.ViewModel;

namespace MagicBookProject;

public partial class PlayPage : ContentPage
{
	bool lever = true;
	public PlayPage(PlayViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
	private async void AnimatedText(Label text)
	{
		string downloadText = "� ��� ����� �������, ������� �������� ���������e �� ����� ������� ����� �� �����";
		if (lever)
		{
			text.Text = string.Empty;
            foreach (char c in downloadText)
            {
                if (lever)
                    lever = false;
                text.Text += c;
                await Task.Delay(100);
            }
            lever = true;
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