using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

using DAL.Contexts;
using DAL.Repository;
using Service;
using Mapper;
using RabbitMq;
using AutoMapper;

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

string? queueName = configuration["QueueName"];

if (queueName is null)
{
	string message = "Configuration variable \"QueueName\" is required";
	Console.Error.WriteLine(message);
	return;
}

string? classroomsDatabaseConnectionString = configuration.GetConnectionString("ClassroomsDatabase");

if (classroomsDatabaseConnectionString is null)
{
	string message = "Connection string \"ClassroomsDatabase\" is required";
	Console.Error.WriteLine(message);
	return;
}

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddDbContext<ClassroomContext>(options => options.UseNpgsql(classroomsDatabaseConnectionString));

builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<IShortBuildingInfoRepository, ShortBuildingInfoRepository>();

builder.Services.AddHostedService(serviceProvider =>
	new RabbitMqConsumer
	(
		rabbitMqHostName,
		Int32.Parse(rabbitMqPort),
		queueName,
		serviceProvider,
		serviceProvider.GetRequiredService<IMapper>()
	)
);

builder.Services.AddScoped<IClassroomService, ClassroomService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.DescribeAllParametersInCamelCase();
	options.CustomSchemaIds(type => type.ToString());

	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Classroom API",
		Description = "Web API отвечающее за создание аудиторий.",
		Contact = new OpenApiContact
		{
			Name = "Telegram разработчика",
			Url = new Uri("https://t.me/Valetra")
		},
		License = new OpenApiLicense
		{
			Name = "Лицензия MIT",
			Url = new Uri("https://opensource.org/license/mit")
		}
	});

	string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
