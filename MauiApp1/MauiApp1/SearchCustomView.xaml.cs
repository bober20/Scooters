using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1;

public partial class SearchCustomView : ContentView
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource), typeof(IEnumerable<string>), typeof(SearchCustomView));
    
    private static readonly BindableProperty EntryTextProperty = BindableProperty.Create(
        nameof(EntryText), typeof(string), typeof(SearchCustomView), string.Empty);
    
    private static readonly BindableProperty FilteredItemsProperty = BindableProperty.Create(
        nameof(FilteredItems), typeof(IEnumerable<string>), typeof(SearchCustomView));
    
    public SearchCustomView()
    {
        InitializeComponent();
        BindingContext = this;
    }
    
    public IEnumerable<string> ItemsSource
    {
        get => (IEnumerable<string>)GetValue(ItemsSourceProperty);
        set
        {
            SetValue(ItemsSourceProperty, value);
            FilteredItems = ItemsSource;
        }
    }

    public IEnumerable<string> FilteredItems
    {
        get => (IEnumerable<string>)GetValue(FilteredItemsProperty);
        set => SetValue(FilteredItemsProperty, value);
    }
    
    public string EntryText
    {
        get => (string)GetValue(EntryTextProperty);
        set
        {
            OnPropertyChanged();
            SetValue(EntryTextProperty, value);
            if (!string.IsNullOrWhiteSpace(value))
            {
                var filteredItems = ItemsSource?.Where(item => item.StartsWith(value)) ?? new List<string>();
                FilteredItems = filteredItems;
            }
            else
            {
                FilteredItems = ItemsSource;
            }
        }
    }
}