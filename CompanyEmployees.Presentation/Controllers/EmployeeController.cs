using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace CompanyEmployees.Presentation.Controllers;

[ApiController]
[Route("api/companies/{companyId:guid}/employees")] // Implemented as a child resource
public class EmployeeController : ControllerBase
{
    private readonly IServiceManager _service;

    public EmployeeController(IServiceManager service) => _service = service;

    [HttpGet]
    // From Query allows uri query collection
    public async Task<IActionResult> GetEmployees(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
    {
        var pagedResult = await _service.EmployeeService.GetEmployees(companyId, employeeParameters, false);
        
        // You can use the Text Json package for serializing objects
        Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(pagedResult.metaData));
        
        return Ok(pagedResult.employees);
    }

    [HttpGet("{id:guid}", Name = "EmployeeById")]
    public async Task<IActionResult> GetEmployee(Guid companyId, Guid id)
    {
        var employee = await _service.EmployeeService.GetEmployee(companyId, id, false);
        return Ok(employee);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        var employeeDto = await _service.EmployeeService.CreateEmployee(companyId, employee, false);
        return CreatedAtRoute("EmployeeById", new { companyId, id = employeeDto.Id }, employeeDto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid id)
    {
        await _service.EmployeeService.DeleteEmployee(companyId, id, false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployee(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        await _service.EmployeeService.UpdateEmployee(companyId, id, employee, false, true);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PatchEmployee(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto> employee)
    {
        if (employee is null) return BadRequest("Body is not present");

        var updated = await _service.EmployeeService.GetEmployeeForPatch(companyId, id, false, true);
        
        // Applies changes to a complete DTO
        employee.ApplyTo(updated.employeeToPatch, ModelState);
        // Adding the ModelState merges any unexpected values therefore breaking validation
        
        TryValidateModel(updated.employeeToPatch); // Validated the now patched employee
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Merges the modified to the old
        await _service.EmployeeService.SaveChangesForPatch(updated.employeeToPatch, updated.employeeEntity);
        return NoContent();
    }
}