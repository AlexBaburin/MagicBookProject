using MagicBookProject.ViewModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Plugin.Maui.Audio;

namespace MagicBookProject
{
    public partial class MainPage : ContentPage
    {
        private IAudioManager audioManager;
        public static IAudioPlayer player;

        public MainPage(MainViewModel vm, IAudioManager audioManager)
        {
            InitializeComponent();
            BindingContext = vm;
            this.audioManager = audioManager;
            PlayBackgroundMusic();
            SetSpeed();
        }
        private async void SetSpeed()
        {
            string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "speed.txt");
            string line = await ReadTextFile("/speed.txt");
            switch(line)
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
        }
        public async Task<string> ReadTextFile(string targetFileName)
        {
            targetFileName = targetFileName.Remove(0, 1);
            string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, targetFileName);
            using FileStream InputStream = System.IO.File.OpenRead(targetFile);
            using StreamReader reader = new StreamReader(InputStream);
            string? line = await reader.ReadLineAsync();
            if (line == "")
                line = "0";
            if (line == "Быстраяая")
                line = "Быстрая";
            string line2 = line;
            return line2;
        }

        private async void PlayBackgroundMusic()
        {
            player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("test.mp3"));
            player.Loop = true;
            player.Volume = 0.5;
            player.Play();
        }
    }

}
