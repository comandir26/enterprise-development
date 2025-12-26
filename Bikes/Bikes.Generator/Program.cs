using Bikes.Generator;
using Bikes.Generator.Options;
using Bikes.ServiceDefaults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<GeneratorOptions>(
    builder.Configuration.GetSection("Generator"));
builder.Services.Configure<KafkaOptions>(
    builder.Configuration.GetSection("Kafka"));

builder.Services.AddSingleton<ContractGenerator>();
builder.Services.AddSingleton<IKafkaProducerFactory, KafkaProducerFactory>();
builder.Services.AddSingleton<KafkaProducerService>();

builder.Services.AddHostedService(provider =>
    provider.GetRequiredService<KafkaProducerService>());

var host = builder.Build();

await host.RunAsync();