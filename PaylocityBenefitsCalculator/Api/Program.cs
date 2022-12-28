using Api.Repository.Contracts;
using Api.Repository;
using Microsoft.OpenApi.Models;
using Api.DataContext;
using Api.Extension;
using Microsoft.Extensions.Logging;
using Api.CustomException;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperApplicationContext>();
builder.Services.AddSingleton<EmployeeDbContext>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPayCheckRepository, PayCheckRepository>();
builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<IDependentRepository, DependentRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Employee Benefit Cost Calculation Api",
        Description = "Api to support employee benefit cost calculations"
    });
});

var allowLocalhost = "allow localhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowLocalhost,
        policy  =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
        });
});

var app = builder.Build();
app.UseMiddleware<HttpResponseCustomExceptionHandler>();
app.ConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(allowLocalhost);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


/*
Application Setting
1. Install the following package

Install-Package Microsoft.EntityFrameworkCore -Version 6.0.5
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 6.0.5
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 6.0.5
Install-Package Dapper
Install-Package Microsoft.Data.SqlClient
2. Create database named 'benefitpaycheckdb' in sql locally
3.Run Migration - Add migration/update database

Follow reository pattern, create commonrepository to minimize code duplication
Change Relation enum to RelationshipType class, creating data base table for relationshiptype make table normalized

 */