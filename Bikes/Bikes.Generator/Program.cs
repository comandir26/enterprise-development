using Bikes.Generator;
using Bikes.Generator.Options; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<GeneratorOptions>(
            context.Configuration.GetSection("Generator"));
        services.Configure<KafkaOptions>(
            context.Configuration.GetSection("Kafka"));

        services.AddSingleton<ContractGenerator>();
        services.AddSingleton<IKafkaProducerFactory, KafkaProducerFactory>();

        services.AddSingleton<KafkaProducerService>();
        services.AddHostedService(provider => provider.GetRequiredService<KafkaProducerService>());

        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });
    })
    .Build();

await host.RunAsync();