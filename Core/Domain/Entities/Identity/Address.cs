namespace Domain.Entities.Identity;

public class Address
{
    public int Id { get; set; }
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Country { get; set; } = String.Empty;
    public string Street { get; set; } = String.Empty;
    public User User { get; set; } 
    public string UserId { get; set; } = String.Empty;
}