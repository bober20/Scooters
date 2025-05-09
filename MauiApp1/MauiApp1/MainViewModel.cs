using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private List<int> _items = [1, 2, 3,45,6,7,4,353,453];
    [ObservableProperty] private string _placeholder = "fdjsfhkdf";
}