namespace MauiApp1;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            DropDown.ItemsSource = CustomSearch.ItemsSource = new List<string>()
            {
                "aaafdjhf",
                "aafjdkjshf",
                "adshhfh",
                "bdjfdjkf",
                "nvsjdhid"
            };
        });
    }
}