
using WorkerService;
using WorkerService.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;     

        services.AddHostedService<Worker>();

        services.AddTransient<IConectaBanco, ConectaBanco>();
        services.AddTransient<ILogEmailRepository, LogEmailRepository>();
    })
    .Build();

host.Run();
