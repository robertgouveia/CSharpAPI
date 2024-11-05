using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyEmployees.Presentation.ActionFilters;

public class ValidationFilterAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var action = context.RouteData.Values["action"];
        var controller = context.RouteData.Values["controller"]; // we can fetch route data

        var param = context.ActionArguments
            .SingleOrDefault(x => x.Value!.ToString()!.Contains("Dto")).Value;

        if (param is null) // Stops route entry for null Dtos
        {
            context.Result = new BadRequestObjectResult($"Object is null: {controller} : {action}");
            return;
        }

        // Catches invalid ModelState before route entry
        if (!context.ModelState.IsValid) context.Result = new UnprocessableEntityObjectResult(context.ModelState);
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}