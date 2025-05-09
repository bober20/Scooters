using System.Windows.Input;

namespace MauiApp1;

public partial class Custom : ContentView
{
    public ICommand DropDownTappedCommand { get; private set; }
    
    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder), typeof(string), typeof(Custom), string.Empty);

    public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(
        nameof(StrokeThickness), typeof(int), typeof(Custom), 1);
    
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource), typeof(IEnumerable<int>), typeof(Custom));
    
    private static readonly BindableProperty IsDropDownOpenProperty = BindableProperty.Create(
        nameof(IsDropDownOpen), typeof(bool), typeof(Custom), false);
    
    public static readonly BindableProperty DropDownBackgroundColorProperty = BindableProperty.Create(
        nameof(DropDownBackgroundColor), typeof(Color), typeof(Custom), Colors.Black);
    
    public static BindableProperty DropDownBorderColorProperty = BindableProperty.Create(
        nameof(DropDownBorderColor), typeof(Color), typeof(Custom), Colors.Black);
    
    public static readonly BindableProperty DropDownClosedIconProperty = BindableProperty.Create(
        nameof(DropDownClosedIcon), typeof(string), typeof(Custom), "\ue705");
    
    public static readonly BindableProperty DropDownOpenedIconProperty = BindableProperty.Create(
        nameof(DropDownOpenedIcon), typeof(string), typeof(Custom), "\ue708");
    
    public Custom()
    {
        InitializeComponent();
        DropDownTappedCommand = new Command(DropDownTapped);
    }
    
    public void DropDownTapped()
    {
        IsDropDownOpen = !IsDropDownOpen;
    }
    
    public string DropDownClosedIcon
    {
        get => (string)GetValue(DropDownClosedIconProperty);
        set => SetValue(DropDownClosedIconProperty, value);
    }
    
    public string DropDownOpenedIcon
    {
        get => (string)GetValue(DropDownOpenedIconProperty);
        set => SetValue(DropDownOpenedIconProperty, value);
    }

    public Color DropDownBackgroundColor
    {
        get => (Color)GetValue(DropDownBackgroundColorProperty);
        set => SetValue(DropDownBackgroundColorProperty, value);
    }
    
    public Color DropDownBorderColor
    {
        get => (Color)GetValue(DropDownBorderColorProperty);
        set => SetValue(DropDownBorderColorProperty, value);
    }
    
    public bool IsDropDownOpen
    {
        get => (bool)GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }
    
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public int StrokeThickness
    {
        get => (int)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }
    
    public IEnumerable<int> ItemsSource
    {
        get => (IEnumerable<int>)GetValue(ItemsSourceProperty);
        set
        {
            SetValue(ItemsSourceProperty, value);
            OnPropertyChanged();
        }
    }

    // public int CornerRadius
    // {
    //     get => (int)GetValue(CornerRadiusProperty);
    //     set => SetValue(CornerRadiusProperty, value);
    // }
}