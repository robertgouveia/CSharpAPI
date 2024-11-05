using CompanyEmployees;
using CompanyEmployees.Extensions;
using CompanyEmployees.Presentation.ActionFilters;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using Services.DataShaping;
using Shared.DataTransferObjects;

// Creates a formatter for JSON Patch Requests
NewtonsoftJsonPatchInputFormatter getJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
        .Services.BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>().First();

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// custom configurations
builder.Services.ConfigureCors();
builder.Services.ConfigureIisIntegration();
builder.Services.ConfigureLoggingService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program)); // allows for automapping
builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // Custom Global Error Handler
builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

builder.Services.AddScoped<ValidationFilterAttribute>(); // Custom Action Filter

//Allow for custom error handling with controller
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true; // Allow for Accept header
        config.ReturnHttpNotAcceptable = true; // If unable to format response it will throw error (instead of JSON)
        config.InputFormatters.Insert(0, getJsonPatchInputFormatter()); // Allows for a patch formatter
    })
    .AddXmlDataContractSerializerFormatters() // Allows for XML
    .AddCustomCSVFormatter() // Custom CSV Formatter
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly); // Adding our external controllers

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// -- Not needed with the new IExceptionHandler that allows for DI --
//var logger = app.Services.GetRequiredService<ILoggerManager>(); How we can get a service
//app.ConfigureExceptionHandler(logger); For using in the exception handler

app.UseExceptionHandler(options => { }); // Default exception middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage(); -- removed as it can interfere with exception handling
}
else
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();