using Goldenwood;
using Goldenwood.Service;

//TODO: Napsat testy pro nov� business metody
//TODO: Sp� ne� pomoc� jm�na by to cht�lo jednotky/budovy identifikovat ID�kem

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ResourcesService, ResourcesService>();
builder.Services.AddSingleton<BuildingService, BuildingService>();
builder.Services.AddSingleton<MilitaryService, MilitaryService>();
builder.Services.AddSingleton<ApplicationDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
