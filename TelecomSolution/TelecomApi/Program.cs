var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () =>
{
    return Results.Ok(new { Message = "Welcome to the Telecom Api" });
});

app.MapPost("/new-hires", (NewEmployeeRequest request) =>
{
    // work their magic.
    var response = new NewEmployeeResponse
    {
        Id = request.Id,
        EmailAddress = $"{request.FirstName.Trim().ToLower()}_{request.LastName.Trim().ToLower()}@company.com",
        PhoneExtension = new Random().Next(100, 999).ToString()
    };
    return Results.Ok(response);
});

app.Run();


public record NewEmployeeRequest
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}
public record NewEmployeeResponse
{
    public int Id { get; set; }
    public string PhoneExtension { get; init; } = string.Empty;
    public string EmailAddress { get; init; } = string.Empty;
}



//{
//    "firstName": "Bob",
//    "lastName": "Smith",
//    "id": 99,
//    "department": "DEV"
//}