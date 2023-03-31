using OnaxTools.Dto;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OnaxTools.Services.RabbitMq
{
    public interface IMessageQueueService
    {
        Task<GenResponse<string>> FetchAndProcessMsgFromQueue(FetchQueueProps props, CancellationToken ct = default, [CallerMemberName] string caller = "");
        Task<GenResponse<bool>> RabbitMqPublish<T>(T data, MqPublisherProps props, CancellationToken ct = default, [CallerMemberName] string caller = "");
    }

    public class MessageQueueService : IMessageQueueService
    {

        private readonly QueueSettings _appsettings;
        private readonly IConnectionFactory _factory;

        public MessageQueueService(string conString = "")
        {
            if (_appsettings == null)
            {
                _appsettings = new ProgramRegistry().QueueSettings();
            }
            _factory = new ConnectionFactory()
            {
                Uri = string.IsNullOrWhiteSpace(conString) ? new Uri(uriString: _appsettings.QueueConfig.QueueConString) : new Uri(conString),
            };
        }

        public async Task<GenResponse<string>> FetchAndProcessMsgFromQueue(FetchQueueProps props, CancellationToken ct = default, [CallerMemberName] string caller = "")
        {
            string objResp = string.Empty;
            string error = string.Empty;
            try
            {
                if (props == null || string.IsNullOrWhiteSpace(props.ClientProvidedName))
                {
                    return await Task.Run(() => GenResponse<string>.Failed("Invalid ClientProvidedName identifier provided."));
                }
                _factory.ClientProvidedName = props.ClientProvidedName;
                using var connection = _factory.CreateConnection();
                using IModel channel = connection.CreateModel();

                channel.ExchangeDeclare(props.ExchangeName, ExchangeType.Direct);
                channel.QueueDeclare(props.QueueName, durable: props.IsDurable.Value, exclusive: props.IsExclusive.Value
                    , autoDelete: props.IsAutoDelete.Value, arguments: props.Arguments);
                channel.QueueBind(queue: props.QueueName, exchange: props.ExchangeName, routingKey: props.RoutingKey, arguments: props.Arguments);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, args) =>
                {
                    var body = args.Body.ToArray();
                    objResp = Encoding.UTF8.GetString(body);
                    if (props.IsAutoAcknowledged.Value)
                    {
                        channel.BasicAck(args.DeliveryTag, multiple: false);
                    }
                    Console.WriteLine($"\n===>{nameof(FetchAndProcessMsgFromQueue)} Successful Run!!!\n");
                };
                string consumerTag = await Task.Run(() => channel.BasicConsume(queue: props.QueueName, autoAck: props.IsAutoAcknowledged.Value, consumer), ct);
                channel.BasicCancel(consumerTag);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"[{caller}]-[{nameof(FetchAndProcessMsgFromQueue)}] ::");
                error = ex.Message;
            }
            return !string.IsNullOrWhiteSpace(objResp) ? GenResponse<string>.Success(objResp) : GenResponse<string>.Failed($"No message found or {error}");
        }

        public async Task<GenResponse<bool>> RabbitMqPublish<T>(T data, MqPublisherProps props, CancellationToken ct = default!, [CallerMemberName] string caller = "")
        {
            try
            {
                if (props == null || string.IsNullOrWhiteSpace(props.ClientProvidedName))
                {
                    return await Task.Run(() => GenResponse<bool>.Failed("Invalid ClientProvidedName identifier provided."));
                }
                _factory.ClientProvidedName = props.ClientProvidedName;
                using var connection = _factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(props.ExchangeName, ExchangeType.Direct);
                channel.QueueDeclare(props.QueueName,
                    durable: props.IsDurable.Value, exclusive: props.IsExclusive.Value, autoDelete: props.IsAutoDelete.Value, arguments: props.Arguments);
                channel.QueueBind(queue: props.QueueName, exchange: props.ExchangeName, routingKey: props.RoutingKey, arguments: props.Arguments);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
                await Task.Run(() => channel.BasicPublish(exchange: props.ExchangeName, routingKey: props.RoutingKey, null, body: body), ct);
                Console.WriteLine($"\n===>{nameof(RabbitMqPublish)} Successful Publish!!!\n");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, $"[{caller}]-[{nameof(RabbitMqPublish)}] ::");
                return GenResponse<bool>.Failed(ex.Message);
            }
            return GenResponse<bool>.Success(true);
        }

    }

    /// <summary>
    /// FetchQueueProps - of type IModel.QueueDeclare
    /// 
    /// </summary>
    public class FetchQueueProps
    {
        [Required] public string QueueName { get; set; }
        [Required] public virtual string ExchangeName { get; set; }
        [Required] public virtual string ClientProvidedName { get; set; }
        [Required] public string RoutingKey { get; set; }

        /// <summary>
        /// Only required to be specified for message consumption. If set as true, this acknowledges the message, which inturn removes it from the queue on consumption.
        /// </summary>
        public bool? IsAutoAcknowledged { get; set; } = true;
        public bool? IsExclusive { get; } = false;
        public bool? IsDurable { get; } = true;
        public bool? IsAutoDelete { get; set; } = false;
        public IDictionary<string, object> Arguments { get; set; } = null;

    }

    public class MqPublisherProps
    {
        [Required] public string QueueName { get; set; }
        [Required] public virtual string ExchangeName { get; set; }
        [Required] public virtual string ClientProvidedName { get; set; }
        [Required] public string RoutingKey { get; set; }
        public bool? IsExclusive { get; } = false;
        public bool? IsDurable { get; } = true;
        public bool? IsAutoDelete { get; set; } = false;
        public IDictionary<string, object> Arguments { get; set; } = null;

    }
}
