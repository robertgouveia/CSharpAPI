using System.Linq.Dynamic.Core;
using Entities.Models;

namespace Repository.Extensions;

// Take queries and push altered queries out
public static class RepositoryEmployeeExtension
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)
        => employees.Where(e => e.Age >= minAge && e.Age <= maxAge);

    public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string? search)
        => string.IsNullOrWhiteSpace(search) ? employees : employees.Where(e => e.Name!.ToLower().Contains(search.Trim().ToLower()));

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string sortInput)
    {
        if (string.IsNullOrWhiteSpace(sortInput)) return employees.OrderBy(e => e.Name);
        var queryString = OrderByQueryBuilder.CreateOrderQuery<Employee>(sortInput);
        
        // System.LINQ.Dynamic will allow a string input
        return string.IsNullOrWhiteSpace(queryString) ? employees.OrderBy(e => e.Name) :
            employees.OrderBy(queryString);
    }
}