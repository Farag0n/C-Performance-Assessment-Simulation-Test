namespace SkyTravel.Models;

public class Flight
{
    public int Id { get; set; }
    public string FlightCode { get; set; }
    public string Origin { get; set; }
    public string Destination  { get; set; }
    public DateTime Departure { get; set; }
    public DateTime Arrival { get; set; }
    public string Capacity { get; set; }

    public FlightStates State { get; set; }

    public enum FlightStates
    {
        Programado,
        EnVuelo,
        Finalizado,
        Cancelado
        
    }
    
    public Flight(){}

    public Flight(int id, string flightCode, string origin, string destination, DateTime departure, DateTime arrival, string capacity)
    {
        Id = id;
        FlightCode = flightCode;
        Origin = origin;
        Destination = destination;
        Departure = departure;
        Arrival = arrival;
        Capacity = capacity;
    }
}