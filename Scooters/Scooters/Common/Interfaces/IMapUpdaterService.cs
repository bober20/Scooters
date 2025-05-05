using Microsoft.Maui.Controls.Maps;

namespace Scooters.Common.Interfaces;

public interface IMapUpdaterService
{
    void SetPins(IEnumerable<Pin> pins);
    void MoveTo(Location center, double latSpan = 0.01);
    void Zoom(double zoomLevel);
}