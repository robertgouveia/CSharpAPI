using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Formatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv")); // Allowing CSV
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode); // You can allow encoding types
    }

    protected override bool CanWriteType(Type? type)
    {
        // If it is not a type of either Company Dto || IEnumerable Company Dto then it is not accepted
        if (!typeof(CompanyDto).IsAssignableFrom(type) && !typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type)) return false;
        
        return base.CanWriteType(type);
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        // Requires a line at a time
        if (context.Object is not IEnumerable<CompanyDto> dtos)
        {
            FormatCsv(buffer, (CompanyDto)context.Object!);
        }
        else
        {
            foreach (var company in dtos)
            {
                FormatCsv(buffer, company);
            }
        }

        // Writes response from the buffer
        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, CompanyDto company)
    {
        // Writes a company to the output
        buffer.AppendLine($"{company.Id},\"{company.Name},\"{company.FullAddress}\"");
    }
}