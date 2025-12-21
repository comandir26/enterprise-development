using Confluent.Kafka;

namespace Bikes.Generator;

public interface IKafkaProducerFactory
{
    public IProducer<Null, string> CreateProducer();
}