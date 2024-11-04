namespace Shared.DataTransferObjects;

// Records do not allow for mutation
// Records cannot be easily serialized into XML
//[Serializable]
//public record CompanyDto(Guid Id, string Name, string FullAddress);

// Init only property setter
public record CompanyDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? FullAddress { get; init; }
}