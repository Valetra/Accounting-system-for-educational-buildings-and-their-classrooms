using Microsoft.EntityFrameworkCore;

using DAL.Contexts;
using DAL.Repository;
using Service;
using Mapper;
using RabbitMq;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

string? rabbitMqHostName = configuration["RabbitMqHostName"];

if (rabbitMqHostName is null)
{
	string message = "Configuration variable \"RabbitMqHostName\" is required";
	Console.Error.WriteLine(message);
	return;
}

string? rabbitMqPort = configuration["RabbitMqPort"];

if (rabbitMqPort is null)
{
	string message = "Configuration variable \"RabbitMqPort\" is required";
	Console.Error.WriteLine(message);
	return;
}

string? buildingsDatabaseConnectionString = configuration.GetConnectionString("BuildingsDatabase");

if (buildingsDatabaseConnectionString is null)
{
	string message = "Connection string \"BuildingsDatabase\" is required";
	Console.Error.WriteLine(message);
	return;
}

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddDbContext<BuildingContext>(options => options.UseNpgsql(buildingsDatabaseConnectionString));

builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();

builder.Services.AddScoped<IBuildingService, BuildingService>();

builder.Services.AddScoped<IRabbitMqProducer, RabbitMqProducer>(serviceProvider => new RabbitMqProducer(rabbitMqHostName, Int32.Parse(rabbitMqPort)));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.DescribeAllParametersInCamelCase();
	options.CustomSchemaIds(type => type.ToString());
});

WebApplication app = builder.Build();

using IServiceScope serviceScope = app.Services.CreateScope();
await serviceScope.ServiceProvider.GetRequiredService<IRabbitMqProducer>().ExchangesDeclare();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
