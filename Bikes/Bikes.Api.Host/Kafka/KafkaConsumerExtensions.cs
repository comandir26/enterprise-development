using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Bikes.Api.Host.Kafka;

/// <summary>
/// Provides extension methods for configuring Kafka consumer services
/// </summary>
public static class KafkaConsumerExtensions
{
    /// <summary>
    /// Adds and configures Kafka consumer services to the service collection
    /// </summary>
    /// <param name="services">The service collection to configure</param>
    /// <returns>The configured service collection</returns>
    public static IServiceCollection AddKafkaConsumer(this IServiceCollection services)
    {
        services.AddOptions<KafkaConsumerOptions>()
            .Configure<IConfiguration>((options, configuration) =>
            {
                configuration.GetSection("Kafka").Bind(options);

                var aspireKafkaConnection = configuration.GetConnectionString("kafka");

                if (!string.IsNullOrEmpty(aspireKafkaConnection))
                {
                    options.BootstrapServers = aspireKafkaConnection;
                }

                else if (string.IsNullOrEmpty(options.BootstrapServers))
                {
                    options.BootstrapServers = "localhost:9092";
                }
            });

        services.AddSingleton<IConsumer<Ignore, string>>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<KafkaConsumerOptions>>().Value;
            var logger = provider.GetRequiredService<ILogger<KafkaConsumer>>();

            var config = new ConsumerConfig
            {
                BootstrapServers = options.BootstrapServers,
                GroupId = options.GroupId,
                EnableAutoCommit = options.EnableAutoCommit,
                AutoOffsetReset = (AutoOffsetReset)options.AutoOffsetReset,

                ApiVersionRequest = false,
                BrokerVersionFallback = "0.10.0.0",
                ApiVersionFallbackMs = 0,
                SecurityProtocol = SecurityProtocol.Plaintext,

                SocketTimeoutMs = 30000,
                SessionTimeoutMs = 30000,
                MetadataMaxAgeMs = 300000,

                AllowAutoCreateTopics = false,
                EnablePartitionEof = true
            };

            var retryCount = 0;
            while (retryCount < options.MaxRetryAttempts)
            {
                try
                {
                    var consumer = new ConsumerBuilder<Ignore, string>(config)
                        .SetErrorHandler((_, error) =>
                        {
                            if (error.IsFatal)
                                logger.LogError("Kafka Fatal Error: {Reason} (Code: {Code})",
                                    error.Reason, error.Code);
                            else if (error.Code != ErrorCode.Local_Transport) 
                                logger.LogWarning("Kafka Warning: {Reason} (Code: {Code})",
                                    error.Reason, error.Code);
                        })
                        .Build();

                    logger.LogInformation("Kafka consumer created successfully for bootstrap servers: {BootstrapServers}",
                        options.BootstrapServers);
                    return consumer;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    logger.LogWarning(ex,
                        "Failed to create Kafka consumer (attempt {RetryCount}/{MaxRetries})",
                        retryCount, options.MaxRetryAttempts);

                    if (retryCount >= options.MaxRetryAttempts)
                    {
                        logger.LogError(ex, "Max retry attempts reached for Kafka consumer");
                        throw;
                    }

                    Thread.Sleep(options.RetryDelayMs);
                }
            }

            throw new InvalidOperationException("Failed to create Kafka consumer");
        });

        services.AddHostedService<KafkaConsumer>();
        return services;
    }
}