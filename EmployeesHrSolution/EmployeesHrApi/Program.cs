// This is Main .NET
using AutoMapper;
using EmployeesHrApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var employeesConnectionString = builder.Configuration.GetConnectionString("employees") ?? throw new Exception("Need a Connection String");

builder.Services.AddDbContext<EmployeeDataContext>(options =>
{
    options.UseSqlServer(employeesConnectionString);
});

var mapperConfig = new MapperConfiguration(opt =>
{
    opt.AddProfile<EmployeesHrApi.AutomapperProfiles.Employees>();
    opt.AddProfile<EmployeesHrApi.AutomapperProfiles.HiringRequestProfile>();
});
var mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton<IMapper>(mapper);
builder.Services.AddSingleton<MapperConfiguration>(mapperConfig);

// above this is configuration for the "behind the scenes" thing in your API
var app = builder.Build();
// after this is setting up "Middleware" - that's the code that receives the HTTP Request and makes the response.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // the code that will let you GET the documentation
    app.UseSwaggerUI(); // this is the middleware that provides the UI for viewing that documentation
}

app.UseAuthorization();


app.MapControllers(); // When we are doing "Controller Based" APIs, this is where it creates the "lookup table" (route table)

app.Run(); // this is when our API is up and running. And it "blocks" here.