using System.Reflection;
using System.Text;

namespace Repository.Extensions;

public static class OrderByQueryBuilder
{
    public static string CreateOrderQuery<T>(string sortInput)
    {
        var sortParams = sortInput.Trim().Split(','); // Splitting sort's via comma
        
        // Getting all the properties of type
        var propertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        // Example Query = Param Asc/Desc, Param Asc/Desc,
        var query = new StringBuilder();
        foreach (var param in sortParams)
        {
            if (string.IsNullOrWhiteSpace(param)) continue; // Ignore empty params

            var property = param.Split(" ")[0]; // Only expect first word
            
            // Get the matching property from entity
            var objectProperty = propertyInfo.FirstOrDefault(p => p.Name.Equals(property, StringComparison.InvariantCultureIgnoreCase));
            
            if (objectProperty is null) continue;

            // Each param can have a space with an order following
            var dir = param.EndsWith(" desc") ? "descending" : "ascending";

            // Example (param desc/asc)
            query.Append($"{objectProperty.Name} {dir},");
        }

        return query.ToString().TrimEnd(','); // This joins the string but removed trailing comma
    }
}