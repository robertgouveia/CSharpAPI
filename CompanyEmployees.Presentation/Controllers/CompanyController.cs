using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    public async Task<IActionResult> GetCompanies()
    {
        // Test Exception Handler
        // throw new Exception("Exception");
        
        var companies = await _service.CompanyService.GetAllCompanies(false);
            
        // Auto JSON response
        return Ok(companies);
        // No need for try catch due to exception handler picking up any errors
    }

    [HttpGet("collection/{ids}", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanies([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
    {
        var companies = await _service.CompanyService.GetCompanies(ids, false);
        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")] // Setting a name for the action
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _service.CompanyService.GetCompany(id, false);
        return Ok(company);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))] // Custom Action Filter
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto? company) // FromBody JSON
    {
        var companyInstance = await _service.CompanyService.Create(company!);
        // Adds a location header with the action and params
        return CreatedAtRoute("CompanyById", new { id = companyInstance.Id }, companyInstance);
    }

    [HttpPost("collection")]
    [ServiceFilter(typeof(ValidationFilterAttribute))] // Custom Action Filter
    public async Task<IActionResult> CreateCompanies([FromBody] IEnumerable<CompanyForCreationDto> companies)
    {
        var created = await _service.CompanyService.CreateCompanies(companies);
        return CreatedAtRoute("CompanyCollection", new { ids = created.companyIds }, created.companyDtos);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _service.CompanyService.DeleteCompany(id, false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
    {
        await _service.CompanyService.UpdateCompany(id, company, true);
        return NoContent();
    }
}