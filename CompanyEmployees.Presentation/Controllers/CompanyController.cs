using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

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

    [HttpGet("{id:guid}", Name = "CompanyById")] // Setting a name for the action
    public IActionResult GetCompany(Guid id)
    {
        var company = _service.CompanyService.GetCompany(id, false);
        return Ok(company);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto? company) // FromBody JSON
    {
        if (company is null) return BadRequest("Body is not present");
        
        var companyInstance = _service.CompanyService.Create(company);
        
        // Adds a location header with the action and params
        return CreatedAtRoute("CompanyById", new { id = companyInstance.Id }, companyInstance);
    }
}