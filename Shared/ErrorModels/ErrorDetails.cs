using System.Text.Json;

namespace eCommerce.WebAPI.ErrorModels;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public override string ToString() => JsonSerializer.Serialize(this);
}