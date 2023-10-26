using Goldenwood;
using Goldenwood.Service;
using GoldenwoodApi.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Goldenwood API",
        Description = "API for the Goldenwood game"
});
// using System.Reflection;
var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddSingleton<ResourcesService, ResourcesService>();
builder.Services.AddSingleton<BuildingService, BuildingService>();
builder.Services.AddSingleton<MilitaryService, MilitaryService>();
builder.Services.AddSingleton<ApplicationDbContext, ApplicationInMemoryDbContext>();


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
DBInitializer.Seed(app);
app.Run();
