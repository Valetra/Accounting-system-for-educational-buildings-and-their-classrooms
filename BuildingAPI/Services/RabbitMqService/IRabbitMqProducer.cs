namespace RabbitMq;

public interface IRabbitMqProducer
{
	Task ExchangesDeclare();
	Task SendMessage(object obj, string exchangeName);
}
