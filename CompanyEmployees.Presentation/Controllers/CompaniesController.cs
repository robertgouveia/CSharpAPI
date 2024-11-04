using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[ApiController]
[Route("api/companies")] // More specific
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;

    // We need access to the services
    public CompaniesController(IServiceManager service) => _service = service;

    [HttpGet]
    public IActionResult GetCompanies()
    {
        try
        {
            var companies = _service.CompanyService.GetAllCompanies(false);
            
            // Auto JSON response
            return Ok(companies);
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}