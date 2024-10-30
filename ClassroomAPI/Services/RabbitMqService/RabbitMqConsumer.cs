using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

using DAL.Repository;
using DAL.Models;
using AutoMapper;

namespace RabbitMq;

public class RabbitMqConsumer : BackgroundService
{
	private readonly IMapper _mapper;
	private readonly IServiceProvider _serviceProvider;
	private readonly string _queueName;
	private readonly IConnection _connection;
	private readonly IModel _channel;

	public RabbitMqConsumer(string hostName, int port, string queueName, IServiceProvider serviceProvider, IMapper mapper)
	{
		_serviceProvider = serviceProvider;
		_mapper = mapper;
		_queueName = queueName;
		ConnectionFactory factory = new() { HostName = hostName, Port = port };
		_connection = factory.CreateConnection();
		_channel = _connection.CreateModel();

		_channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

		ExchangesDeclare();

		_channel.QueueBind(_queueName, "create", String.Empty);
		_channel.QueueBind(_queueName, "update", String.Empty);
		_channel.QueueBind(_queueName, "delete", String.Empty);
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		stoppingToken.ThrowIfCancellationRequested();

		EventingBasicConsumer consumer = new(_channel);

		consumer.Received += async (ch, ea) =>
		{
			using (IServiceScope serviceScope = _serviceProvider.CreateScope())
			{
				IShortBuildingInfoRepository shortBuildingInfoRepository = serviceScope.ServiceProvider.GetRequiredService<IShortBuildingInfoRepository>();

				string content = Encoding.UTF8.GetString(ea.Body.ToArray());

				JsonDeserializeBuilding? jsonDeserializeBuilding = JsonSerializer.Deserialize<JsonDeserializeBuilding>(content);

				if (jsonDeserializeBuilding is not null)
				{
					ShortBuildingInfo shortBuildingInfo = _mapper.Map<ShortBuildingInfo>(jsonDeserializeBuilding);

					switch (ea.Exchange)
					{
						case "create":
							await shortBuildingInfoRepository.Create(shortBuildingInfo);
							break;
						case "update":
							await shortBuildingInfoRepository.Update(shortBuildingInfo);
							break;
						case "delete":
							await shortBuildingInfoRepository.Delete(shortBuildingInfo.Id);
							break;
						default:
							break;
					}
				}

				_channel.BasicAck(ea.DeliveryTag, false);
			};
		};

		_channel.BasicConsume(_queueName, false, consumer);

		return Task.CompletedTask;
	}

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
	public override void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
	{
		_channel.Close();
		_connection.Close();
		base.Dispose();
	}

	private Task ExchangesDeclare()
	{
		_channel.ExchangeDeclare("create", ExchangeType.Direct);
		_channel.ExchangeDeclare("update", ExchangeType.Direct);
		_channel.ExchangeDeclare("delete", ExchangeType.Direct);

		return Task.CompletedTask;
	}
}
