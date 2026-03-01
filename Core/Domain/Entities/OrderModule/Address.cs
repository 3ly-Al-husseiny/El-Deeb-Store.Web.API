namespace Domain.Entities.OrderModule;

public class Address
{

    public Address()
    {
        
    }
    
    public Address(string firstName, string lastName, string street , string city , string country)
    {
        FirstName = firstName;
        LastName = lastName;
        Street = street;
        City = city;
        Country = country;
    }
    
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Country { get; set; } = String.Empty;
    public  string City { get; set; } = String.Empty;
    public string Street { get; set; } = String.Empty;
}