namespace MauiApp1;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Viewssss.ItemsSource = new List<string>()
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5"
            };
        });
    }
}