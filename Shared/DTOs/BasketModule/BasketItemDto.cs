using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.BasketModule;

public class BasketItemDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    [Range(1,double.MaxValue)]
    public decimal Price { get; set; }
    public string PictureUrl { get; set; } = string.Empty;
    [Range(1,10)]
    public int Quantity { get; set; }
}