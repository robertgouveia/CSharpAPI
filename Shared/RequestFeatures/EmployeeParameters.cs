namespace Shared.RequestFeatures;

public class EmployeeParameters : RequestParameters
{
    public uint MinAge { get; set; }
    public uint MaxAge { get; set; } = int.MaxValue; // default

    // Ensures user is entering a minimum lower than the maximum
    public bool ValidAgeRange => MaxAge > MinAge;
}