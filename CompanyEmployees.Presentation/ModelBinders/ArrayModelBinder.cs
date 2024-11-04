using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyEmployees.Presentation.ModelBinders;

public class ArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        // If the data type is not Enumerable the result = failed
        if (!bindingContext.ModelMetadata.IsEnumerableType)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        // Get the Enumeration Value
        var providedValue = bindingContext.ValueProvider
            .GetValue(bindingContext.ModelName)
            .ToString();

        // If the value is null return null
        if (string.IsNullOrEmpty(providedValue))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        // Checks if the string can be turned to a generic type
        var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
        
        // Creates a converter for the specified type
        var converter = TypeDescriptor.GetConverter(genericType);

        // Converts strings in the Enumeration to the type
        var objectArray = providedValue.Split([","], StringSplitOptions.RemoveEmptyEntries)
            .Select(x => converter.ConvertFromString(x.Trim()))
            .ToArray();

        // Create memory of the type and length
        var guidArray = Array.CreateInstance(genericType, objectArray.Length);
        objectArray.CopyTo(guidArray, 0); // Copy Object Array to type GUID Array
        bindingContext.Model = guidArray; // Setting the value to the GUID Array
        
        // Returning the model
        bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        return Task.CompletedTask;
    }
}