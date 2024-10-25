namespace RabbitMq;

public interface IRabbitMqService
{
	Task SendMessage(object obj, string exchangeName);
}
