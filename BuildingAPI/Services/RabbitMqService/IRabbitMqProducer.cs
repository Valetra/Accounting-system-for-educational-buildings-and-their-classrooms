namespace RabbitMq;

public interface IRabbitMqProducer
{
	Task DeclareExchanges();
	Task SendMessage(object obj, string exchangeName);
}
