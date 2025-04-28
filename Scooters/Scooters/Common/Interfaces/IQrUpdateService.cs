namespace Scooters.Common.Interfaces;

public interface IQrUpdaterService
{
    public void DisconnectQrHandler();
    public void ConnectQrHandler();
}