using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMq;

public class RabbitMqProducer(string hostName, int port) : IRabbitMqProducer
{
	public Task DeclareExchanges()
	{
		ConnectionFactory factory = new() { HostName = hostName, Port = port };
		using IConnection connection = factory.CreateConnection();
		using IModel channel = connection.CreateModel();

		channel.ExchangeDeclare("create", ExchangeType.Direct);
		channel.ExchangeDeclare("update", ExchangeType.Direct);
		channel.ExchangeDeclare("delete", ExchangeType.Direct);

		return Task.CompletedTask;
	}

	public async Task SendMessage(object obj, string exchangeName)
	{
		string message = JsonSerializer.Serialize(obj);
		await SendMessage(message, exchangeName);
	}

	private Task SendMessage(string message, string exchangeName)
	{
		ConnectionFactory factory = new() { HostName = hostName, Port = port };
		using IConnection connection = factory.CreateConnection();
		using IModel channel = connection.CreateModel();

		byte[] body = Encoding.UTF8.GetBytes(message);

		channel.BasicPublish(exchange: exchangeName,
					   routingKey: String.Empty,
					   basicProperties: null,
					   body: body);

		return Task.CompletedTask;
	}
}

