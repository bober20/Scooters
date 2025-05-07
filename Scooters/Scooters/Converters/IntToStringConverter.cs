using System.Globalization;

namespace Scooters.Converters;

public class IntCollectionToStringCollectionConverter : IValueConverter
{
    const string leaveEmptyText = "Leave empty";
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable<int> intCollection)
        {
            return intCollection.Select(i => i == 0 ? leaveEmptyText : i.ToString()).ToList();
        }
        return Enumerable.Empty<string>();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable<string> stringCollection)
        {
            return stringCollection.Select(s => 
                s == leaveEmptyText ? 0 : 
                int.TryParse(s, out int val) ? val : 0
            ).ToList();
        }
        
        return 0;
    }
}