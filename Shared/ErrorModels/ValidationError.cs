namespace eCommerce.WebAPI.ErrorModels;

public class ValidationError
{
    public string Field { get; set; } = string.Empty;
    public IEnumerable<string> Errors { get; set; } = [];
}