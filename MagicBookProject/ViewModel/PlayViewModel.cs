using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicBookProject.ViewModel
{
    public partial class PlayViewModel : ObservableObject
    {
        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
            Vibration.Vibrate(100);
        }
    }
}
