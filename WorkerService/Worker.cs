using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using WorkerService.Entities;
using WorkerService.Repositories;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _factory;
        private readonly ILogEmailRepository _logEmailRepository;
        public Worker(
            ILogger<Worker> logger, 
            IConfiguration configuration, 
            ILogEmailRepository logEmailRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _logEmailRepository = logEmailRepository;

            _factory = new ConnectionFactory()
            {
                HostName = _configuration.GetSection("BrokerConfig")["Servidor"] ?? string.Empty,
                UserName = _configuration.GetSection("BrokerConfig")["Usuario"] ?? string.Empty,
                Password = _configuration.GetSection("BrokerConfig")["Senha"] ?? string.Empty
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var connection = _factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {


                    channel.QueueDeclare(queue: "filaEmail",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);


                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (sender, args) =>
                    {
                        var body = args.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var interesse = JsonSerializer.Deserialize<Interesse>(message);

                        var para = interesse.Livro.Doador.Email;
                        var conteudo = $"Olá {interesse.Livro.Doador.Nome}. " +
                            $"O seu livro {interesse.Livro.Nome} recebeu uma nova solicitação de interesse.";

                        new Email().SendEmail(para, "Nova solicitação de Interesse.", conteudo);

                        _logger.LogInformation($"E-mail enviado para {interesse.Livro.Doador.Email}", DateTimeOffset.Now);

                        _logEmailRepository.Inserir(new LogEmail(interesse.Livro.Doador.Email, conteudo));

                    };

                    channel.BasicConsume(
                            queue: "filaEmail",
                            autoAck: true,
                            consumer: consumer);                    
                }
                
            }
        }
    }
}
