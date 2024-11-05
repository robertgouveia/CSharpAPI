using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

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

    [HttpGet("{id:guid}", Name = "EmployeeById")]
    public IActionResult GetEmployee(Guid companyId, Guid id)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, id, false);
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        if (employee is null) return BadRequest("Body is not present");

        if (!ModelState.IsValid) return UnprocessableEntity(ModelState); // Checks for model validation

        var employeeDto = _service.EmployeeService.CreateEmployee(companyId, employee, false);
        return CreatedAtRoute("EmployeeById", new { companyId, id = employeeDto.Id }, employeeDto);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployee(Guid companyId, Guid id)
    {
        _service.EmployeeService.DeleteEmployee(companyId, id, false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployee(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        if (employee is null) return BadRequest("Body is not present");

        if (!ModelState.IsValid) return UnprocessableEntity(ModelState); // Check for validation
        
        _service.EmployeeService.UpdateEmployee(companyId, id, employee, false, true);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public IActionResult PatchEmployee(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto> employee)
    {
        if (employee is null) return BadRequest("Body is not present");

        var updated = _service.EmployeeService.GetEmployeeForPatch(companyId, id, false, true);
        
        // Applies changes to a complete DTO
        employee.ApplyTo(updated.employeeToPatch, ModelState);
        // Adding the ModelState merges any unexpected values therefore breaking validation
        
        TryValidateModel(updated.employeeToPatch); // Validated the now patched employee
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Merges the modified to the old
        _service.EmployeeService.SaveChangesForPatch(updated.employeeToPatch, updated.employeeEntity);
        return NoContent();
    }
}