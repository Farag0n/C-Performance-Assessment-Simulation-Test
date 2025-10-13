namespace SkyTravel.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? DocNumber { get; set; }
    public string Email { get; set; }
    public string CelNumber { get; set; }
    
    public User(){}

    public User(int id, string name, string docNumber, string email, string celNumber)
    {
        Id = id;
        Name = name;
        DocNumber = docNumber;
        Email = email;
        CelNumber = celNumber;
    }
}