using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[ApiController]
[Route("api/companies/{companyId:guid}/employees")] // Implemented as a child resource
public class EmployeeController : ControllerBase
{
    private readonly IServiceManager _service;

    public EmployeeController(IServiceManager service) => _service = service;

    [HttpGet]
    public IActionResult GetEmployees(Guid companyId)
    {
        var employees = _service.EmployeeService.GetEmployees(companyId, false);
        return Ok(employees);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetEmployee(Guid companyId, Guid id)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, id, false);
        return Ok(employee);
    }
}