using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace MagicBookProject.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [RelayCommand]
    async Task TapInfo()
    {
        await Shell.Current.GoToAsync(nameof(InfoPage));
        Vibration.Vibrate(100);
    }

    [RelayCommand]
    async Task TapSettings()
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
        Vibration.Vibrate(100);
    }
    [RelayCommand]
    async Task TapPlay()
    {
        await Shell.Current.GoToAsync(nameof(PlayPage));
        Vibration.Vibrate(100);
    }
}
