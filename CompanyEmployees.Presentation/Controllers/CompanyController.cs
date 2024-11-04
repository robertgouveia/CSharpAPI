using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[ApiController]
[Route("api/companies")] // Plural
public class CompanyController : ControllerBase
{
    private readonly IServiceManager _service;

    // We need access to the services
    public CompanyController(IServiceManager service) => _service = service;

    [HttpGet]
    public IActionResult GetCompanies()
    {
        // Test Exception Handler
        // throw new Exception("Exception");
        
        var companies = _service.CompanyService.GetAllCompanies(false);
            
        // Auto JSON response
        return Ok(companies);
        // No need for try catch due to exception handler picking up any errors
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _service.CompanyService.GetCompany(id, false);
        return Ok(company);
    }
}