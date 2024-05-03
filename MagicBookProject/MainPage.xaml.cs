using MagicBookProject.ViewModel;
using Plugin.Maui.Audio;
namespace MagicBookProject
{
    public partial class MainPage : ContentPage
    {
        private IAudioManager audioManager;
        private IAudioPlayer player;

        public MainPage(MainViewModel vm, IAudioManager audioManager)
        {
            InitializeComponent();
            BindingContext = vm;
            this.audioManager = audioManager;
            PlayBackgroundMusic();
        }
        private async void PlayBackgroundMusic()
        {
            player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("test.mp3"));
            player.Loop = true;
            player.Play();
        }
    }

}
