namespace Shared.DataTransferObjects;

//[Serializable]
//public record EmployeeDto(Guid Id, string Name, int Age, string Position);

public record EmployeeDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public int Age { get; init; }
    public string? Position { get; init; }
}