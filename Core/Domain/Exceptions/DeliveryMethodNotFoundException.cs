namespace Domain.Exceptions;

public class DeliveryMethodNotFoundException : NotFoundException
{
    public DeliveryMethodNotFoundException(int id) : base($"Delivery Method with {id} is not found")
    {
    }
}