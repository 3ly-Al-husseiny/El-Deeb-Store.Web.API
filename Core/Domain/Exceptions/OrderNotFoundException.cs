namespace Domain.Exceptions;

public class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(Guid id) : base($"Order with id {id} is not found")
    {
    }
}