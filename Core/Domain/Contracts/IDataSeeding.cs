namespace Services.Abstraction;

public interface IDataSeeding // Interface for data seeding , Implement to deal with Database --> Persistence layer
{
    Task SeedAsync(); // Method to perform data seeding
    Task SeedIdentityAsync(); // Method to perform identity data seeding (e.g., roles, users)
}