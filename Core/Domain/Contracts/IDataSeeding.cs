namespace Services.Abstraction;

public interface IDataSeeding // Interface for data seeding , Implement to deal with Database --> Persistence layer
{
    Task SeedAsync(); // Method to perform data seeding
}