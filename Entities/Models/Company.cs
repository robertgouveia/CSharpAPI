using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Company
{
    [Column("CompanyId")] // specifies name in DB
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Company Name is a required field")]
    [MaxLength(60, ErrorMessage = "Company Name must be less than 60 characters")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Company Address is a required field")]
    [MaxLength(60, ErrorMessage = "Company Address must be less than 60 characters")]
    public string? Address { get; set; }
    
    public string? Country { get; set; }
    public ICollection<Employee>? Employees { get; set; }
}