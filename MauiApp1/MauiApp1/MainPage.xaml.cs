namespace MauiApp1;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            DropDown.ItemsSource = new List<int>()
            {
                1,2,3,4,5,6
            };
        });
    }
}