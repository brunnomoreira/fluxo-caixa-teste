using FluxoCaixa.Domain.Observer.Events;
using FluxoCaixa.Domain.Observer.Handlers;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Observer.Manager
{
    public class RabbitMQEventManager : IEventManager
    {
        private readonly List<Type> _eventTypes;
        protected readonly Dictionary<string, List<IHandler>> _handlers;

        public RabbitMQEventManager()
        {
            _eventTypes = new List<Type>();
            _handlers = new Dictionary<string, List<IHandler>>();
        }

        public void Publish(IEvent @event)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;
                channel.QueueDeclare(eventName, true, false, false, null);
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", eventName, null, body);
                Debug.WriteLine($"{eventName} Published");
            }
        }

        public void Subscribe(Type type, IHandler handler)
        {
            var eventName = type.Name;

            if (!_eventTypes.Contains(type))
            {
                _eventTypes.Add(type);
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<IHandler>());
            }

            _handlers[eventName].Add(handler);

            StartBasicConsume(type);
        }

        private void StartBasicConsume(Type type)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, DispatchConsumersAsync = true };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = type.Name;
            channel.QueueDeclare(eventName, true, false, false, null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                List<IHandler> handlers = _handlers[eventName];
                foreach (IHandler handler in handlers)
                {
                    if (handler == null) continue;
                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    await handler.Handle((IEvent)@event);
                }
            }
        }
    }
}
