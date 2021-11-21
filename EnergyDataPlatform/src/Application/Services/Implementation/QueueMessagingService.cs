using EnergyDataPlatform.src.Application.Services.Interfaces;
using EnergyDataPlatform.src.Data.Mesaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Services.Implementation
{
    public class QueueMessagingService : BackgroundService
    {

        private IConnection _connection;
        private IModel _channel;
        private ISensorMeasurementService _sensorMeasurementService;

        public QueueMessagingService(IServiceProvider serviceProvider)
        {
            _sensorMeasurementService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ISensorMeasurementService>();
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory { Uri = new Uri("amqps://kirltfqj:4sABkRnTegiS9L6wLcmZgijy4sWlnFHX@goose.rmq2.cloudamqp.com/kirltfqj") };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("energy-platform-queue-exchange", ExchangeType.Topic);
            _channel.QueueDeclare("energy-platform-queue", false, false, false, null);
            _channel.QueueBind("energy-platform-queue", "energy-platform-queue-exchange", "energy-platform-queue", null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var messageData = JsonSerializer.Deserialize<Message>(message);
                Console.WriteLine(messageData.ToString());
                HandleMessage(messageData);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume("energy-platform-queue", false, consumer);
            return Task.CompletedTask;
        }

        private void HandleMessage(Message message)
        {
            _sensorMeasurementService.AddSensorMeasurementFromExternalQueue(message);
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
