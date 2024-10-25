using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMq;

public class RabbitMqService(string hostName) : IRabbitMqService
{
	public async Task SendMessage(object obj, string exchangeName)
	{
		string message = JsonSerializer.Serialize(obj);
		await SendMessage(message, exchangeName);
	}

	private Task SendMessage(string message, string exchangeName)
	{
		ConnectionFactory factory = new() { HostName = hostName, Port = 5673 };
		using IConnection connection = factory.CreateConnection();
		using IModel channel = connection.CreateModel();

		channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

		byte[] body = Encoding.UTF8.GetBytes(message);

		channel.BasicPublish(exchange: exchangeName,
					   routingKey: String.Empty,
					   basicProperties: null,
					   body: body);

		return Task.CompletedTask;
	}
}
