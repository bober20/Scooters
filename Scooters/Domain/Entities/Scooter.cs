namespace Domain.Entities;

public class Scooter
{
    public Guid Id { get; set; }
    public string ModelDescription { get; set; }
    public Coordinates Coordinates { get; set; }
    public List<Reservation> Reservations { get; set; }
    public List<Ride> Rides { get; set; }
}