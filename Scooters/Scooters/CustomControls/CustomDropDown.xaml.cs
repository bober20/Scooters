using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace Scooters.CustomControls;

public partial class CustomDropDownView : ContentView
{
    public ICommand DropDownTappedCommand { get; private set; }
    
    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder), typeof(string), typeof(CustomDropDownView), string.Empty);

    public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(
        nameof(StrokeThickness), typeof(int), typeof(CustomDropDownView), 1);
    
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource), typeof(IEnumerable<string>), typeof(CustomDropDownView), null);
    
    public static readonly BindableProperty IsDropDownOpenProperty = BindableProperty.Create(
        nameof(IsDropDownOpen), typeof(bool), typeof(CustomDropDownView), false);
    
    // public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
    //     nameof(CornerRadius), typeof(int), typeof(CustomDropDownView), 0,
    //     propertyChanged: (bindable, value, newValue) =>
    //     {
    //         (Border)bindable.
    //     });
    
    
    // Keep the existing method
    public void DropDownTapped()
    {
        IsDropDownOpen = !IsDropDownOpen;
    }
    
    public CustomDropDownView()
    {
        InitializeComponent();
        BindingContext = this;
        
        DropDownTappedCommand = new Command(DropDownTapped);
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
    
    public IEnumerable<string> ItemsSource
    {
        get => (IEnumerable<string>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    
    // public int CornerRadius
    // {
    //     get => (int)GetValue(CornerRadiusProperty);
    //     set => SetValue(CornerRadiusProperty, value);
    // }
}