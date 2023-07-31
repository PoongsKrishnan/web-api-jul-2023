var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// above are config
var app = builder.Build();
// after this is middleware

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // code to get documentation
    app.UseSwaggerUI(); // ui to view Documentation
}

app.UseAuthorization();


app.MapControllers(); // creates lookup table (route table)

app.Run(); // API runs
