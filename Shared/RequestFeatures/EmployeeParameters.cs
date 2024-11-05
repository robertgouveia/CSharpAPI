namespace Shared.RequestFeatures;

public class EmployeeParameters : RequestParameters
{
    public EmployeeParameters() => OrderBy = "Name"; // Default
    
    public uint MinAge { get; set; }
    public uint MaxAge { get; set; } = int.MaxValue; // default
    public string? Search { get; set; }

    // Ensures user is entering a minimum lower than the maximum
    public bool ValidAgeRange => MaxAge > MinAge;
}