using System.Text.Json;

namespace Entities.ErrorModel;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }

    // Allows ToString() to return the JSON format
    public override string ToString() => JsonSerializer.Serialize(this);
}