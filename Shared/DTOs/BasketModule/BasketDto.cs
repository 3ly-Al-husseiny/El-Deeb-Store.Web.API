namespace Shared.DTOs.BasketModule;

public record BasketDto
{
    // init --> to make the property immutable after initialization
    public string Id { get; init; }
    
    public ICollection<BasketItemDto> BasketItemDtos { get; init; } = [];
}