using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using MyMicroservice.Data;
using MyMicroservice.Models;
using System.Text;


namespace MyMicroservice.Services
{
  public class RabbitMQConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            var factory = new ConnectionFactory() 
            { 
                HostName = "serveo.net",
                Port = 5672,  // Puerto estándar de RabbitMQ para conexiones
                UserName = "guest",  // Usuario predeterminado
                Password = "guest" 
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "visitas", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var visitData = JsonConvert.DeserializeObject<VisitData>(message);
                if (visitData != null)
                {
                    SaveVisitToDatabase(visitData);
                }
            };

            _channel.BasicConsume(queue: "visitas", autoAck: true, consumer: consumer);
        }

        private void SaveVisitToDatabase(VisitData visitData)
        {
            // Crear un nuevo alcance de servicio para acceder al contexto de la base de datos
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Crear una nueva instancia de la entidad Visita
                try
                {
                    var visita = new Visita
                    {
                        tipo_elemento = visitData.tipo_elemento,
                        id_elemento = visitData.id_elemento,
                        fecha_visita = visitData.fecha_visita.ToUniversalTime()
                    };

                    dbContext.Visitas.Add(visita);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Registra el error
                    Console.WriteLine($"Error al guardar en la base de datos: {ex.Message}");
                } // Guardar los cambios en la base de datos
            }
        }
    }

    // Modelo para los datos recibidos
    public class VisitData
    {
        public string? tipo_elemento { get; set; }
        public int id_elemento { get; set; }
        public DateTime fecha_visita { get; set; }
    }  // Tu clase RabbitMQConsumer o RabbitMQProducer aquí
}

