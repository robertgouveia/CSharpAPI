namespace Shared.DataTransferObjects;

// Records do not allow for mutation
public record CompanyDto(Guid Id, string Name, string FullAddress);