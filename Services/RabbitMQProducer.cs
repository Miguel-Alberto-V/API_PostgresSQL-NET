using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using MyMicroservice.Data;
using MyMicroservice.Models;
using System.Text;

namespace MyMicroservice.Services
{
   
public class RabbitMQProducer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQProducer()
    {
        var factory = new ConnectionFactory() 
        { 
            HostName = "serveo.net",
            Port = 5672,  // Puerto estándar de RabbitMQ para conexiones
            UserName = "guest",  // Usuario predeterminado
            Password = "guest" 
        };  // Contraseña predeterminada }; // Cambia según tu configuración
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "visitas",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void SendVisit(string tipo_elemento, int id_elemento)
    {
        var visitData = new
        {
            tipo_elemento,
            id_elemento,
            fecha_visita = DateTime.Now
        };

        var message = JsonConvert.SerializeObject(visitData);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
                             routingKey: "visitas",
                             basicProperties: null,
                             body: body);
    }

    public void Close()
    {
        _channel.Close();
        _connection.Close();
    }
}
}
