namespace SkyTravel.Models;

public class ReservFlight
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public int UserId { get; set; }
    public PayStatus Pay { get; set; }
    
    public enum PayStatus
    {
        Pagado,
        Pendiente
    }
    
    public DateOnly ReservationDate { get; set; }
    
    //Claves foraneas
    public User? User { get; set; }
    public Flight? Flight { get; set; }
    
    public ReservFlight(){ }

    public ReservFlight(int id,  int flighId, int userId, PayStatus pay)
    {
        Id = id;
        FlightId = flighId;
        UserId = userId;
        Pay = pay;
    }
}