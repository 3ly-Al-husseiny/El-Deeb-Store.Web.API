namespace Domain.Entities.Shared;

public class BasedEntity<TKey> // Generic Class [BaseClass with Generic Type Parameter]
{
    public TKey Id { get; set; }
}