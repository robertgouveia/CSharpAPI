using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public abstract record CompanyForManipulation
{
    [Required(ErrorMessage = "Name is a required field")]
    [MaxLength(30, ErrorMessage = "Name must be less than 30 characters")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Address is a required field")]
    public string? Address { get; init; }

    [Required(ErrorMessage = "Country is a required field")]
    [MaxLength(20, ErrorMessage = "Country must be less than 20 characters")]
    public string? Country { get; init; }
    
    public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
}