// Удалите или закомментируйте эту строку:
// using Bikes.Generator;

// Добавьте это в начало файла:
using Bikes.Generator;
using Bikes.Generator.Options; // Если создадите папку Options
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Регистрируем конфигурацию
        services.Configure<GeneratorOptions>(
            context.Configuration.GetSection("Generator"));
        services.Configure<KafkaOptions>(
            context.Configuration.GetSection("Kafka"));

        // Регистрируем сервисы
        services.AddSingleton<ContractGenerator>();
        services.AddSingleton<IKafkaProducerFactory, KafkaProducerFactory>();

        // Регистрируем BackgroundService
        services.AddHostedService<KafkaProducerService>();

        // Настраиваем логирование
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });
    })
    .Build();

await host.RunAsync();