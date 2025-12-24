using Confluent.Kafka;

namespace Bikes.Generator;

/// <summary>
/// Factory interface for creating Kafka producers
/// </summary>
public interface IKafkaProducerFactory
{
    /// <summary>
    /// Creates and returns a Kafka producer instance
    /// </summary>
    /// <returns>Configured Kafka producer</returns>
    public IProducer<Null, string> CreateProducer();
}