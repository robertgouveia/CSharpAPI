using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

// Abstracted properties to reduce duplicated code
public abstract record EmployeeForManipulationDto
{
    [Required(ErrorMessage = "Name is a required field")]
    [MaxLength(30, ErrorMessage = "Name must be less than 30 characters")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Age is a required field")]
    //Without the below the integer will be set to 0 if not present
    [Range(18, int.MaxValue, ErrorMessage = "Age is required and cannot be less than 18")]
    public int Age { get; init; }

    [Required(ErrorMessage = "Position is a required field")]
    [MaxLength(20, ErrorMessage = "Position must be less than 20 characters")]
    public string? Position { get; init; }
}