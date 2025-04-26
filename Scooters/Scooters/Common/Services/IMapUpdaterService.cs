using Microsoft.Maui.Controls.Maps;

namespace Scooters.Common.Services;

public interface IMapUpdaterService
{
    void SetPins(IEnumerable<Pin> pins);
    void MoveTo(Location center, double latSpan = 0.01, double lonSpan = 0.01);
}