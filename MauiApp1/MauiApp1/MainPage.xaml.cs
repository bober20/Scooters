namespace MauiApp1;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
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