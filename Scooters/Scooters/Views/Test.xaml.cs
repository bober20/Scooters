using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scooters.Views;

public partial class Test : ContentPage
{
    public Test()
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