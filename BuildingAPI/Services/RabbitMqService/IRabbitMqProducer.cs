namespace RabbitMq;

public interface IRabbitMqProducer
{
	Task InitRabbitMq();
	Task SendMessage(object obj, string exchangeName);
}
