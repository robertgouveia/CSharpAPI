using System.Dynamic;
using System.Reflection;
using Contracts;

namespace Services.DataShaping;

public class DataShaper<T> : IDataShaper<T> where T : class
{
    public PropertyInfo[] Properties { get; set; }

    public DataShaper()
    {
        // Get the properties of T
        Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }
    
    // Multiple of T
    public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);
        return FetchDataForEntities(entities, requiredProperties);
    }

    public ExpandoObject ShapeData(T entity, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);
        return FetchDataForEntity(entity, requiredProperties);
    }

    // Gets the required properties
    private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldString)
    {
        if (string.IsNullOrWhiteSpace(fieldString)) return Properties.ToList(); // Empty List
        
        // Checks the name of properties and gets the related
        var fields = fieldString.Split(',', StringSplitOptions.RemoveEmptyEntries);
        
        // Gets the related name of the property against the field
        var result = fields.Select(
            field => Properties.FirstOrDefault(p => p.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase)))
            .OfType<PropertyInfo>()
            .ToList();

        return result;
    }

    // Creates Expando Objects
    private IEnumerable<ExpandoObject> FetchDataForEntities(IEnumerable<T> entities, IEnumerable<PropertyInfo> properties)
    => entities.Select(entity => FetchDataForEntity(entity, properties)).ToList();
    
    private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> properties)
    {
        var shapedObject = new ExpandoObject();

        foreach (var property in properties)
        {
            var objectPropertyValue = property.GetValue(entity); // Get the value from the property
            shapedObject.TryAdd(property.Name, objectPropertyValue); // Adds the property and value
        }

        return shapedObject;
    }
}